using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace sibervatanprojemweb.Controllers
{
    public class blogController : ApiController
    {
        private readonly sibervatandbEntities2 _ent = new sibervatandbEntities2();

        [HttpGet]
        public List<blogsayfam> BlogGetir()
        {
            return _ent.blogsayfam
                .OrderByDescending(x => x.id)
                .ToList();
        }

        [HttpGet]
        public IHttpActionResult BlogDetayGetir(int id)
        {
            var blog = _ent.blogsayfam.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        [HttpPost]
        [AdminAuth]
        public IHttpActionResult BlogGetir2(blogsayfam blog)
        {
            var hata = BlogDogrula(blog);
            if (hata != null)
            {
                return BadRequest(hata);
            }

            _ent.blogsayfam.Add(blog);
            _ent.SaveChanges();
            return Ok(BlogGetir());
        }

        [HttpPost]
        [AdminAuth]
        public IHttpActionResult BlogGuncelle(blogsayfam byeni)
        {
            var hata = BlogDogrula(byeni);
            if (hata != null)
            {
                return BadRequest(hata);
            }

            var blog = _ent.blogsayfam.Find(byeni.id);
            if (blog == null)
            {
                return NotFound();
            }

            blog.baslik = byeni.baslik.Trim();
            blog.ozet = byeni.ozet.Trim();
            blog.etiket = byeni.etiket.Trim();
            blog.icerik = byeni.icerik.Trim();
            _ent.SaveChanges();

            return Ok(BlogGetir());
        }

        [HttpPost]
        [AdminAuth]
        public IHttpActionResult blogsil2(int id)
        {
            var blog = _ent.blogsayfam.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            _ent.blogsayfam.Remove(blog);
            _ent.SaveChanges();
            return Ok(BlogGetir());
        }

        private static string BlogDogrula(blogsayfam blog)
        {
            if (blog == null)
            {
                return "Yazi verisi bos olamaz.";
            }

            if (string.IsNullOrWhiteSpace(blog.baslik) ||
                string.IsNullOrWhiteSpace(blog.ozet) ||
                string.IsNullOrWhiteSpace(blog.etiket) ||
                string.IsNullOrWhiteSpace(blog.icerik))
            {
                return "Baslik, ozet, etiket ve icerik alanlari zorunludur.";
            }

            if (blog.baslik.Length > 50)
            {
                return "Baslik en fazla 50 karakter olabilir.";
            }

            if (blog.etiket.Length > 50)
            {
                return "Etiket en fazla 50 karakter olabilir.";
            }

            blog.baslik = blog.baslik.Trim();
            blog.ozet = blog.ozet.Trim();
            blog.etiket = blog.etiket.Trim();
            blog.icerik = blog.icerik.Trim();

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ent.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
