﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YPYA.Models;
using YPYA.Bl;

namespace YPYA.Controllers
{
    public class ProjectController : Controller
    {
        projeyonetimvtEntities db = new projeyonetimvtEntities();
        BusinessLayer bl = new BusinessLayer();
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

        public JsonResult SurecBilgileri(int surecID, int projeID)
        {
            Surec surecBilgi = new Surec();
            surecBilgi = db.Surecs.Where(x => x.Id == surecID).FirstOrDefault();

            string baslangic = surecBilgi.PlanBaslangic?.ToString("yyyy-MM-dd");           
            string bitis = surecBilgi.PlanBitis?.ToString("yyyy-MM-dd");

            List<object> Kisiler = new List<object>();
            int? musteriID = db.Projes.Where(x => x.Id == projeID).FirstOrDefault().MusteriId;
            foreach (Kullanici k in db.Kullanicis)
            {
                if (k.Id == musteriID)
                {
                    continue;
                }
                var jsonKisi = new
                {

                    kisi = k.Adsoyad,
                    kisiID = k.Id,
                };

                Kisiler.Add(jsonKisi);
            }
            var jsonmodel = new {

                surecDetayKisiler = Kisiler,
                surecBaslik = surecBilgi.Baslik,
                projeBaslik = surecBilgi.Proje.Baslik,
                sureciOlusturan = surecBilgi.Proje.Kullanici.Adsoyad,
                surecPlanlananBaslangic = baslangic,
                surecPlanlananBitis = bitis,
                surecTamamlanma = surecBilgi.Tamamlanan,
                
                
            };

            return Json(jsonmodel);
        }
        public JsonResult IsTakibiBilgileri(int surecID)
        {
            List<object> Istakipleri = new List<object>();
           

            foreach (KullaniciSurec item in db.KullaniciSurecs.Where(x => x.SurecId == surecID))
            {    
        

                string surecBaslangic = item.IsTakibi.BaslangicTarihi?.ToString("yyyy-MM-dd");
                string surecBitis = item.IsTakibi.BitisTarihi?.ToString("yyyy-MM-dd");
                string surecTamamlanma = item.IsTakibi.TamamlanmaTarihi?.ToString("yyyy-MM-dd");
                float orn = 0;
                orn = (float)item.IsTakibi.TamamlanmaOranı * 123;
                orn = orn / 100;
                var jsonmodel = new
                {
                    surecDetayKisi = item.Kullanici.Adsoyad,
                    surecDetayRolAdi = item.Rol.RolAdi,
                    surecDetayBaslangic = surecBaslangic,
                    surecDetayKisiID = item.KullaniciId,
                    surecDetayBitis = surecBitis,
                    surecDetayTamamlanma = surecTamamlanma,
                    surecDetayTamamlanmaOrani = orn,

                };

                Istakipleri.Add(jsonmodel);
            }
            return Json(Istakipleri);
          
        }
        public JsonResult IsTakibiKaydet(SurecIstakibi istakibiBilgi ,int surecID)
        {
            List<string> snc = new List<string>();
                       
            if (bl.KullaniciSurecEkle(istakibiBilgi, surecID) == 1)
            {
                snc.Add("Basarili");
            }



            return Json(snc);
        }

    }
}