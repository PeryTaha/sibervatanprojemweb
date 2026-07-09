using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace sibervatanprojemweb.Controllers
{
    public class LoginController : ApiController
    {
        private static readonly ConcurrentDictionary<string, DateTime> AdminTokens = new ConcurrentDictionary<string, DateTime>();
        private static readonly ConcurrentDictionary<string, int> BasarisizGirisler = new ConcurrentDictionary<string, int>();
        private static readonly ConcurrentDictionary<string, DateTime> GirisKilitleri = new ConcurrentDictionary<string, DateTime>();
        private static readonly TimeSpan TokenOmru = TimeSpan.FromHours(8);
        private static readonly TimeSpan KilitSuresi = TimeSpan.FromMinutes(10);
        private const int MaksimumBasarisizGiris = 5;

        public static bool TokenGecerliMi(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            if (!AdminTokens.TryGetValue(token, out var sonKullanim))
            {
                return false;
            }

            if (DateTime.UtcNow - sonKullanim > TokenOmru)
            {
                AdminTokens.TryRemove(token, out _);
                return false;
            }

            AdminTokens[token] = DateTime.UtcNow;
            return true;
        }

        [HttpPost]
        [Route("api/login/kontrol")]
        public IHttpActionResult LoginKontrol([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.kullaniciadi) || string.IsNullOrWhiteSpace(model.sifre))
            {
                return BadRequest("Kullanici adi ve sifre zorunludur.");
            }

            var kullaniciadi = model.kullaniciadi.Trim();
            var sifre = model.sifre;
            var denemeAnahtari = DenemeAnahtariOlustur(kullaniciadi);

            if (GirisKilitliMi(denemeAnahtari))
            {
                return Content((HttpStatusCode)429, "Cok fazla hatali giris denemesi. Biraz sonra tekrar dene.");
            }

            using (var db = new sibervatandbEntities2())
            {
                var kullanici = db.admin.FirstOrDefault(x =>
                    x.kullaniciadi == kullaniciadi && x.sifre == sifre);

                if (kullanici == null)
                {
                    HataliGirisiKaydet(denemeAnahtari);
                    return Unauthorized();
                }

                BasarisizGirisler.TryRemove(denemeAnahtari, out _);
                GirisKilitleri.TryRemove(denemeAnahtari, out _);

                var token = Guid.NewGuid().ToString("N");
                AdminTokens[token] = DateTime.UtcNow;

                return Ok(new { mesaj = "Giris basarili.", token });
            }
        }

        private static bool GirisKilitliMi(string denemeAnahtari)
        {
            if (!GirisKilitleri.TryGetValue(denemeAnahtari, out var kilitBitis))
            {
                return false;
            }

            if (kilitBitis > DateTime.UtcNow)
            {
                return true;
            }

            GirisKilitleri.TryRemove(denemeAnahtari, out _);
            BasarisizGirisler.TryRemove(denemeAnahtari, out _);
            return false;
        }

        private static void HataliGirisiKaydet(string denemeAnahtari)
        {
            var denemeSayisi = BasarisizGirisler.AddOrUpdate(denemeAnahtari, 1, (_, mevcut) => mevcut + 1);
            if (denemeSayisi < MaksimumBasarisizGiris)
            {
                return;
            }

            GirisKilitleri[denemeAnahtari] = DateTime.UtcNow.Add(KilitSuresi);
            BasarisizGirisler.TryRemove(denemeAnahtari, out _);
        }

        private static string DenemeAnahtariOlustur(string kullaniciadi)
        {
            var ip = HttpContext.Current?.Request?.UserHostAddress ?? "unknown";
            return ip + "|" + kullaniciadi.ToLowerInvariant();
        }
    }

    public class LoginModel
    {
        public string kullaniciadi { get; set; }
        public string sifre { get; set; }
    }
}
