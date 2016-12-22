using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YPYA.Models;

namespace YPYA.Controllers
{
    public class HomeController : Controller
    {
        projeyonetimvtEntities db = new projeyonetimvtEntities();

        private void sesAta()
        {
            Session["id"] = 1;
        }

        public ActionResult Index()
        {
            sesAta();
            if (Session["id"] != null)
            {
                int id = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == id);

                ViewBag.olusturulan = db.Projes.Where(x => x.OlusturanKullaniciId == id);
                ViewBag.dahilOlunanSayi = db.ProjeKullanicis.Count(x => x.KullaniciId == id);
                ViewBag.dahilOlunan = db.ProjeKullanicis.Where(x => x.KullaniciId == id);
                return View();
            }
            else return RedirectToAction("Login","Sign");
           
        }

        public ActionResult NewProject(int? id)
        {
            sesAta();
            ViewBag.projGuncelle = null;
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);
                if(id != null && db.Projes.Find(id).Kullanici.Id == kulId)
                    ViewBag.projGuncelle = db.Projes.FirstOrDefault(x => x.Id == id);
                return View();
            }
            else return RedirectToAction("Login", "Sign");
        }

        //Fonksiyonlar
        public ActionResult SurecCek(int id)
        {
            return View(db.Surecs.Where(x => x.ProjeId == id));
        }

        public JsonResult SurecleriCek(int projeId)
        {
            List<object> jsonArray = new List<object>();
            foreach (Surec parent in db.Surecs.Where(x=>x.ProjeId == projeId && x.ParentSurecId == null))
            {
                List<object> processJsonArray = new List<object>();
                foreach (Surec process in db.Surecs.Where(x => x.ProjeId == projeId && x.ParentSurecId == parent.Id))
                {
                    List<object> subProcessJsonArray = new List<object>();

                    foreach (Surec subProcess in db.Surecs.Where(x => x.ProjeId == projeId && x.ParentSurecId == process.Id))
                    {
                        var jsonSubProcessModel = new
                        {
                            surecId = subProcess.Id,
                            surecAdi = subProcess.Baslik
                        };
                        subProcessJsonArray.Add(jsonSubProcessModel);
                    }

                    var jsonProcessModel = new
                    {
                        surecId = process.Id,
                        surecAdi = process.Baslik,
                        altSurec = subProcessJsonArray
                    };
                    processJsonArray.Add(jsonProcessModel);
                }
                var jsonModel = new
                {
                    surecId = parent.Id,
                    surecAdi = parent.Baslik,
                    altSurec = processJsonArray
                };
                jsonArray.Add(jsonModel);
            }
            return Json(jsonArray, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProjeOlustur(string projeAdi, string baslangic, string bitis, int projeId,int butce)
        {
            Proje p;
            if (projeId == 0)
            {
                p = new Proje();
                p.Baslik = projeAdi;
                p.PlanBaslangic = Convert.ToDateTime(baslangic);
                p.PlanBitis = Convert.ToDateTime(bitis);
                p.OlusturanKullaniciId = Convert.ToInt32(Session["id"]);
                p.Olusturulma = DateTime.Now;
                p.Butce = butce;

                db.Projes.Add(p);
            }
            else
            {
                p = db.Projes.FirstOrDefault(x=>x.Id == projeId);
                p.Baslik = projeAdi;
                p.PlanBaslangic = Convert.ToDateTime(baslangic);
                p.PlanBitis = Convert.ToDateTime(bitis);
                p.Butce = butce;
            }
            db.SaveChanges();
            var jsonModel = new
            {
                basari = 1,
                projeId = p.Id
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }

        

        public JsonResult ProjeSil(int projeId)
        {
            Proje p = db.Projes.FirstOrDefault(x=>x.Id == projeId);
            db.Projes.Remove(p);
            db.SaveChanges();
            var jsonModel = new {
                basari = 1
            };
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurecOlustur(string baslik, string baslangic, string bitis, int parentSurecId, int projeId)
        {
            Surec s;
            if (parentSurecId != 0)
            {
                s = new Surec()
                {
                    Baslik = baslik,
                    PlanBaslangic = Convert.ToDateTime(baslangic),
                    PlanBitis = Convert.ToDateTime(bitis),
                    OlusturanId = Convert.ToInt32(Session["id"]),
                    OlusturmaTarihi = DateTime.Now,
                    ParentSurecId = parentSurecId,
                    ProjeId = projeId
                };
            }
            else
            {
                s = new Surec()
                {
                    Baslik = baslik,
                    PlanBaslangic = Convert.ToDateTime(baslangic),
                    PlanBitis = Convert.ToDateTime(bitis),
                    OlusturanId = Convert.ToInt32(Session["id"]),
                    OlusturmaTarihi = DateTime.Now,
                    ProjeId = projeId
                };
            }

            db.Surecs.Add(s);
            db.SaveChanges();

            var jsonModel = new {
                basari = 1,
                surecId = s.Id
            };
            return Json(jsonModel,JsonRequestBehavior.AllowGet);
        }

        public void ProjeRed(int projeId)
        {
            int kulId = Convert.ToInt32(Session["id"]);
            ProjeKullanici pk = db.ProjeKullanicis.FirstOrDefault(x=>x.ProjeId == projeId && x.KullaniciId == kulId);
            db.ProjeKullanicis.Remove(pk);

            Bildirim b = new Bildirim();
            b.KullaniciId = db.Projes.Find(projeId).OlusturanKullaniciId;
            b.Icerik = db.Kullanicis.Find(kulId).Adsoyad + " " + db.Projes.Find(projeId).Baslik + " adlı projenize katılmayı reddetti";
            b.Okundu = false;
            b.Tarih = DateTime.Now;
            b.Link = "#";

            db.Bildirims.Add(b);

            db.SaveChanges();
        }

        public JsonResult BildirimCek()
        {
            int kulId = Convert.ToInt32(Session["id"]);
            List<Object> jsonList = new List<Object>();
            foreach (Bildirim i in db.Bildirims.Where(x=>x.KullaniciId == kulId).OrderByDescending(x=>x.Id))
            {
                var jsonIcerik = new
                {
                    icerik = i.Icerik,
                    okundu = i.Okundu,
                    link = i.Link,
                    tarih = i.Tarih.Value.ToString("dd-MM-yyyy")
                };
                jsonList.Add(jsonIcerik);
            }

            var jsonModel = new
            {
                toplam = db.Bildirims.Count(x => x.KullaniciId == kulId && x.Okundu == false),
                icerikler = jsonList
            };

            return Json(jsonModel);
        }

        public void AcceptProject(int projeId)
        {
            int kulId = Convert.ToInt32(Session["id"]);
            ProjeKullanici pk = db.ProjeKullanicis.FirstOrDefault(x=>x.KullaniciId == kulId && x.ProjeId == projeId);
            pk.Durum = true;

            Bildirim b = new Bildirim();
            b.KullaniciId = db.Projes.Find(projeId).OlusturanKullaniciId;
            b.Icerik = db.Kullanicis.Find(kulId).Adsoyad + " " + db.Projes.Find(projeId).Baslik + " adlı projenize katılmayı kabul etti";
            b.Okundu = false;
            b.Tarih = DateTime.Now;
            b.Link = "#";

            db.Bildirims.Add(b);
            db.SaveChanges();
        }

        public void BildirimOku()
        {
            int kulId = Convert.ToInt32(Session["id"]);
            foreach (Bildirim b in db.Bildirims.Where(x=>x.KullaniciId == kulId))
            {
                b.Okundu = true;
            }

            db.SaveChanges();
        }
    }
}