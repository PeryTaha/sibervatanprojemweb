using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace sibervatanprojemweb.Controllers
{
    public class blogController : ApiController
    {
        sibervatandbEntities2 _ent = new sibervatandbEntities2();

        [HttpGet]
        public List<blogsayfam> BlogGetir()
        {
            return _ent.blogsayfam.ToList();
        }
        [HttpPost]
        public List<blogsayfam> BlogGetir2(blogsayfam blog)
        {
            try
            {
                _ent.blogsayfam.Add(blog);
                _ent.SaveChanges();
                return BlogGetir();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public bool Blogsil(int blogid)
        {
            blogsayfam b = _ent.blogsayfam.Find(blogid);
            if (b != null)
            {
                _ent.blogsayfam.Remove(b);
                _ent.SaveChanges();
                return true;
            }
            return false;
        }


        [HttpPost]
        public bool BlogEkle(blogsayfam b)
        {
            try
            {
                _ent.blogsayfam.Add(b);
                _ent.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        [HttpGet]
        public List<blogsayfam>blogsil2(int id)
        {
            try {
                blogsayfam b = _ent.blogsayfam.Find(id);
                if (b != null)
                {
                    _ent.blogsayfam.Remove(b);
                    _ent.SaveChanges();

                }
                return BlogGetir();
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpPost]
        public bool BlogGuncelle(blogsayfam byeni)
        {
            blogsayfam blg = _ent.blogsayfam.Find(byeni.id);
            {
                try
                {
                    blg.baslik = byeni.baslik;
                    blg.icerik = byeni.icerik;
                    blg.ozet = byeni.ozet;
                    _ent.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    throw; return false;

                }


            }
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
    }

}  