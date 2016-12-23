using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YPYA.Models;

namespace YPYA.Controllers
{
    public class ProjectController : Controller
    {
        projeyonetimvtEntities db = new projeyonetimvtEntities();

        private void sesAta()
        {
            Session["id"] = 1;

        }

        public ActionResult Index(int? id)
        {
            sesAta();
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x => x.Id == id);
                    ViewBag.proje = p;
                    ViewBag.projeId = p.Id;
                    ViewBag.projeAdi = p.Baslik;
                    return View(db.Projes.FirstOrDefault(x => x.Id == id));
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Sign");
           
        }

        public ActionResult ProcessEdit(int? id)
        {
            sesAta();
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x=>x.Id == id);
                    ViewBag.proje = p;
                    ViewBag.projeId = p.Id;
                    ViewBag.projeAdi = p.Baslik;
                    return View(db.Surecs.Where(x=>x.ProjeId == id && x.Kullanici.Id == kulId));
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Sign");
        }

        public ActionResult InvitePeople(int? id)
        {
            sesAta();
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x => x.Id == id);
                    ViewBag.proje = p;
                    ViewBag.projeId = p.Id;
                    ViewBag.projeAdi = p.Baslik;
                    return View(db.ProjeKullanicis.Where(x=>x.ProjeId == id));
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Sign");
        }

        public ActionResult Customer(int? id)
        {
            sesAta();
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x => x.Id == id);
                    ViewBag.proje = p;
                    ViewBag.projeId = p.Id;
                    ViewBag.projeAdi = p.Baslik;

                    return View(db.ProjeKullanicis.Where(x => x.ProjeId == id));
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Sign");
        }

        public ActionResult AddRequest(int? id)
        {
            sesAta();
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x => x.Id == id);
                    ViewBag.proje = p;
                    ViewBag.projeId = p.Id;
                    ViewBag.projeAdi = p.Baslik;
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Sign");
        }

        public PartialViewResult ProjectMenu(int id)
        {
            ViewBag.menuKisi = db.Projes.FirstOrDefault(x => x.Id == id);
            return PartialView();
        }

        public JsonResult SearchPeople(string content)
        {
            List<Object> JsonList = new List<Object>();
            foreach (Kullanici k in db.Kullanicis.Where(x=>x.KullaniciAdi.Contains(content) || x.Adsoyad.Contains(content)))
            {
                var jsonModel = new
                {
                    kulId = k.Id,
                    kulAdi = k.KullaniciAdi,
                    kulAdSoyad = k.Adsoyad
                };
                JsonList.Add(jsonModel);
            }
            return Json(JsonList);
        }

        public int AddCustomer(int projectId, int customerId)
        {
            Proje p = db.Projes.Find(projectId);
            p.MusteriId = customerId;
            db.SaveChanges();
            return 1;
        }

        public int RemoveCustomer(int projectId)
        {
            Proje p = db.Projes.Find(projectId);
            p.MusteriId = null;
            db.SaveChanges();
            return 1;
        }

        public JsonResult AddPeopleProject(int id,int projeId)
        {
            if (!db.ProjeKullanicis.Any(x => x.KullaniciId == id && x.ProjeId == projeId) && db.Projes.Find(projeId).Kullanici.Id != id)
            {
                ProjeKullanici pk = new ProjeKullanici()
                {
                    KullaniciId = id,
                    ProjeId = projeId,
                    Durum = false,
                    Tarih = DateTime.Now
                };

                db.ProjeKullanicis.Add(pk);

                Bildirim b = new Bildirim();
                b.KullaniciId = id;
                b.Okundu = false;
                b.Link = "Home/Index";
                b.Icerik = db.Kullanicis.Find(Convert.ToInt32(Session["id"])).Adsoyad + " sizi " + db.Projes.Find(projeId).Baslik + " adlı projeye davet etti";
                b.Tarih = DateTime.Now;

                db.Bildirims.Add(b);

                db.SaveChanges();
                Kullanici k = db.Kullanicis.Find(id);

                var jsonModel = new
                {
                    durum = 1,
                    kulAdSoyad = k.Adsoyad,
                    kulAdi = k.KullaniciAdi,
                    kulMail = k.Mail
                };

                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Kullanici k = db.Kullanicis.Find(id);

                var jsonModel = new
                {
                    durum = 0,
                    kulAdSoyad = k.Adsoyad
                };

                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
        }

        public int IstekEkle(int projectId, string content, string header)
        {
            int musteriId = Convert.ToInt32(Session["id"]);
            MusteriIsteri m = new MusteriIsteri();
            m.Baslik = header;
            m.ProjeId = projectId;
            m.Icerik = content;
            m.MusteriId = musteriId;
            m.Tarih = DateTime.Now;

            db.MusteriIsteris.Add(m);

            
            Bildirim b = new Bildirim();
            b.Icerik = db.Kullanicis.Find(musteriId).Adsoyad + ", " + db.Projes.Find(projectId).Baslik + " projesine yeni bir istek ekledi";
            b.KullaniciId = db.Projes.Find(projectId).OlusturanKullaniciId;
            b.Okundu = false;
            b.Tarih = DateTime.Now;

            db.Bildirims.Add(b);

            db.SaveChanges();
            
            return 1;
        }

        public JsonResult TumIstekler(int projectId)
        {
            List<object> jsonList = new List<object>();
            foreach (MusteriIsteri m in db.MusteriIsteris.Where(x=>x.ProjeId == projectId))
            {
                var jsonModel = new {
                    icerik = m.Icerik,
                    id = m.Id,
                    kullanici = m.Kullanici.Adsoyad,
                    tarih = m.Tarih.Value.ToString("dd-MM-yyyy")
                };
                jsonList.Add(jsonModel);
            }
            return Json(jsonList);
        }

        public JsonResult IstekCek(int isterId)
        {
            MusteriIsteri m;
            if(isterId == 0) m = db.MusteriIsteris.OrderByDescending(x=>x.Id).FirstOrDefault();
            else m = db.MusteriIsteris.Find(isterId);
            var jsonModel = new
            {
                baslik = m.Baslik,
                icerik = m.Icerik,
                id = m.Id,
                kullanici = m.Kullanici.Adsoyad,
                tarih = m.Tarih.Value.ToString("dd-MM-yyyy")
            };
            return Json(jsonModel);
        }
    }
}