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
        public ActionResult Index()
        {  
             
            Session["id"] = 1;
            if (Session["id"] != null)
            {
                int id = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == id);

                ViewBag.olusturulan = db.Projes.Where(x => x.OlusturanKullaniciId == id);
                return View();
            }
            else return RedirectToAction("Login","Sign");
           
        }

        public ActionResult NewProject(int? id)
        {
            Session["id"] = 1;
            ViewBag.projGuncelle = null;
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);
                if(id != null)
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

        public JsonResult ProjeOlustur(string projeAdi, string baslangic, string bitis, int projeId)
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

                db.Projes.Add(p);
            }
            else
            {
                p = db.Projes.FirstOrDefault(x=>x.Id == projeId);
                p.Baslik = projeAdi;
                p.PlanBaslangic = Convert.ToDateTime(baslangic);
                p.PlanBitis = Convert.ToDateTime(bitis);
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
    }
}