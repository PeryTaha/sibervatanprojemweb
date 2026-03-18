using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors; // Hata verirse NuGet'ten Microsoft.AspNet.WebApi.Cors yükle

namespace sibervatanprojemweb.Controllers
{
    [EnableCors(origins: "https://localhost:44330", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("api/login/kontrol")]
        public IHttpActionResult LoginKontrol([FromBody] LoginModel model)
        {
            
            using (var db = new sibervatandbEntities2())
            {
               
                var kullanici = db.admin.FirstOrDefault(x =>
                    x.kullaniciadi == model.kullaniciadi && x.sifre == model.sifre);

                if (kullanici != null)
                {
                    return Ok(new { mesaj = "Giriş Başarılı!" });
                }
                return Unauthorized();
            }
        }
    }


    public class LoginModel
    {
        public string kullaniciadi { get; set; }
        public string sifre { get; set; }
    }
}