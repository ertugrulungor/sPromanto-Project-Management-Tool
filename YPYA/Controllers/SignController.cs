using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YPYA.Models;
using YPYA.helper;

namespace YPYA.Controllers
{
    public class SignController : Controller
    {
        projeyonetimvtEntities db = new projeyonetimvtEntities();
        Yardimci y = new Yardimci();
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        

        public JsonResult GirisYap(string kullanici, string sifre)
        {
            kullanici = y.PreventXSS(kullanici);
            sifre = y.PreventXSS(sifre);

            sifre = y.MD5Sifrele(sifre);
            if (db.Kullanicis.Any(x => (x.KullaniciAdi == kullanici || x.Mail == kullanici) && x.Sifre == sifre))
            {
                Session["id"] = db.Kullanicis.FirstOrDefault(x => (x.KullaniciAdi == kullanici || x.Mail == kullanici) && x.Sifre == sifre).Id;
                var jsonModel = new {
                    basari = 1
                };
                return Json(jsonModel,JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonModel = new
                {
                    basari = 0
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult KayitOl(string adsoyad, string kuladi, string mail, string sifre)
        {
            adsoyad = y.PreventXSS(adsoyad);
            kuladi = y.PreventXSS(kuladi);
            sifre = y.PreventXSS(sifre);
            mail = y.PreventXSS(mail);

            if (db.Kullanicis.Any(x=> x.KullaniciAdi == kuladi || x.Mail == mail))
            {
                var jsonModel = new
                {
                    basari = 0
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                sifre = y.MD5Sifrele(sifre);
                Kullanici k = new Kullanici() {
                    KullaniciAdi = kuladi,
                    Mail = mail,
                    Sifre = sifre,
                    Tarih = DateTime.Now,
                    Adsoyad = adsoyad
                };
                db.Kullanicis.Add(k);
                db.SaveChanges();
                Session["id"] = k.Id;
                var jsonModel = new
                {
                    basari = 1
                };
                return Json(jsonModel, JsonRequestBehavior.AllowGet);
            }
        }
    }
}