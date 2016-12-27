using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YPYA.Models;
using YPYA.Bl;
using YPYA.helper;

namespace YPYA.Controllers
{
    public class ProjectController : Controller
    {
        projeyonetimvtEntities db = new projeyonetimvtEntities();
        BusinessLayer bl = new BusinessLayer();
        Yardimci y = new Yardimci();
       

        public ActionResult Index(int? id)
        {
            
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

        public ActionResult TeamMember(int? id)
        {
            
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
                    return View(db.KullaniciSurecs.Where(x=>x.KullaniciId == kulId && x.Surec.Proje.Id == id));
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Sign");
        }

        public ActionResult Report(int? id)
        {
            
            if (Session["id"] != null)
            {
                int kulId = Convert.ToInt32(Session["id"]);
                ViewBag.k = db.Kullanicis.FirstOrDefault(x => x.Id == kulId);

                if (id != null)
                {                
                    return View(db.Surecs.Find(id));
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
            content = y.PreventXSS(content);
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
            content = y.PreventXSS(content);
            header = y.PreventXSS(header);
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

        public JsonResult SurecBilgileri(int surecID, int projeID)
        {
            Surec surecBilgi = new Surec();
            surecBilgi = db.Surecs.Where(x => x.Id == surecID).FirstOrDefault();

            string baslangic = surecBilgi.PlanBaslangic?.ToString("yyyy-MM-dd");           
            string bitis = surecBilgi.PlanBitis?.ToString("yyyy-MM-dd");

            List<object> Kisiler = new List<object>();
            int? musteriID = db.Projes.Where(x => x.Id == projeID).FirstOrDefault().MusteriId;
            foreach (ProjeKullanici pk in db.ProjeKullanicis.Where(x => x.ProjeId == projeID && x.Durum == true))
            {
               
                    if (pk.Id == musteriID)
                    {
                        continue;
                    }
                    var jsonKisi = new
                    {
                        kisi = pk.Kullanici.Adsoyad,
                        kisiID = pk.Id,
                    };

                    Kisiler.Add(jsonKisi);
               
            }
            var jsonmodel = new {
                surecDetayKisiler = Kisiler,
                surecBaslik = surecBilgi.Baslik,
                projeBaslik = surecBilgi.Proje.Baslik,
                sureciOlusturan = surecBilgi.Proje.Kullanici1.Adsoyad,
                surecPlanlananBaslangic = baslangic,
                surecPlanlananBitis = bitis,
                surecTamamlanma = surecBilgi.Tamamlanan,
                surecNotu = surecBilgi.Note

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
                
                var jsonmodel = new
                {
                    surecDetayKisi = item.Kullanici.Adsoyad,
                    surecDetayRolAdi = item.Rol.RolAdi,
                    surecDetayBaslangic = surecBaslangic,
                    surecDetayKisiID = item.KullaniciId,
                    surecDetayBitis = surecBitis,
                    surecDetayTamamlanma = surecTamamlanma,
                    surecDetayTamamlanmaOrani = item.IsTakibi.TamamlanmaOranı,
                    id = item.IstakibiId

                };

                Istakipleri.Add(jsonmodel);
            }
            return Json(Istakipleri);
          
        }
        public JsonResult SurecSil(int surecID ,int projeID)
        {
            List<string> snc = new List<string>();

            if (bl.SurecSilme(surecID,projeID) == 1)
            {                                                                    
                snc.Add("Basarili");
            }
            return Json(snc);

        }
        public JsonResult IsTakibiKaydet(SurecIstakibi istakibiBilgi ,int surecID,int projeID,string surecBaslik,string surecNote)
        {
            List<string> snc = new List<string>();
            surecBaslik = y.PreventXSS(surecBaslik);
            surecNote=y.PreventXSS(surecNote);
            if (bl.KullaniciSurecEkle(istakibiBilgi, surecID,projeID,surecBaslik,surecNote) == 1)
            {
                snc.Add("Basarili");
            }
            return Json(snc);
        }
        
        public void AkisSil(int akisid,int surecID,int projeID)
        {
            IsTakibi i = db.IsTakibis.Find(akisid);
            db.IsTakibis.Remove(i);
            db.SaveChanges();
            bl.SurecOranHesapla(surecID);
            bl.ProjeOranDuzenle(projeID);
        }

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);
        public string getGanttData(int projeId)
        {
            string json = "[";

            foreach (Surec process in db.Surecs.Where(x => x.ProjeId == projeId))
            {
                if (process.ParentSurecId != null)
                {
                    json += "{'id':'"+process.Id+"','name':'"+process.Baslik+"','parent':'"+process.ParentSurecId+"','progressValue':'"+process.Tamamlanan+"%','actualStart':"+ (process.PlanBaslangic.Value - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond +", 'actualEnd':"+ (process.PlanBitis.Value - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond + "},";
                }
                else
                {
                    json += "{'id':'" + process.Id + "','name':'" + process.Baslik + "','progressValue':'" + process.Tamamlanan + "%','actualStart':"+ (process.PlanBaslangic.Value - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond + ", 'actualEnd':"+ (process.PlanBitis.Value - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond + "},";
                }

                foreach (KullaniciSurec ks in db.KullaniciSurecs.Where(x=>x.SurecId == process.Id))
                {
                    json += "{'id':'" + ks.Id + "57790802','name':'" + ks.Rol.RolAdi + "','person':'"+ks.Kullanici.Adsoyad+"','parent':'" + process.Id + "','progressValue':'" + ks.IsTakibi.TamamlanmaOranı + "%','actualStart':" + (ks.IsTakibi.BaslangicTarihi.Value - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond + ", 'actualEnd':" + (ks.IsTakibi.BitisTarihi.Value - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond + "},";
                }
            }
            json = json.Remove(json.Length - 1);
            json += "]";
            json = json.Replace("\"", "");
            json = json.Replace("'", "\"");
            return json;
        }


        public JsonResult SurecBilgiKaydet(SurecIstakibi istakibiBilgi,int surecID,int projeID)
        {
            List<string> snc = new List<string>();
            if (bl.SurecBilgiKaydet(istakibiBilgi, surecID, projeID) == 1)
            {
                snc.Add("Basarili");
            }
            return Json(snc);
        }
    }
}