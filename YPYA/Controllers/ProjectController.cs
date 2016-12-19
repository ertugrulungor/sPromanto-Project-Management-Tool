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
        public ActionResult Index(int? id)
        {
            Session["id"] = 1;
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);
                ViewBag.olusturulan = db.Projes.Where(x => x.OlusturanKullaniciId == id);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x => x.Id == id);
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
            Session["id"] = 1;
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x=>x.Id == id);
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
            Session["id"] = 1;
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {
                    Proje p = db.Projes.FirstOrDefault(x => x.Id == id);
                    ViewBag.projeId = p.Id;
                    ViewBag.projeAdi = p.Baslik;
                    return View(db.ProjeKullanicis.Where(x=>x.ProjeId == id));
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

        public JsonResult AddPeopleProject(int id,int projeId)
        {
            if (!db.ProjeKullanicis.Any(x => x.KullaniciId == id && x.ProjeId == projeId))
            {
                ProjeKullanici pk = new ProjeKullanici()
                {
                    KullaniciId = id,
                    ProjeId = projeId,
                    Durum = false,
                    Tarih = DateTime.Now
                };

                db.ProjeKullanicis.Add(pk);
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
                var jsonModel = new
                {
                    durum = 0
                };

                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
        }
    }
}