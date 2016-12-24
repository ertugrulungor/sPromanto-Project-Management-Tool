using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YPYA.Models;

namespace YPYA.Bl
{

    public class BusinessLayer
    {
        public projeyonetimvtEntities db = new projeyonetimvtEntities();

        public int OranKontrol(int oran)
        {
            float fOran = 0;
            if (oran < 0) 
            {
                oran = 0; 
            }
            fOran = oran * 100;
            fOran = fOran / 123;
            return Convert.ToInt32(fOran);
        }

        public void SurecOranHesapla(int surecID)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                float orn = 0;
                int syc = 0;
                foreach (KullaniciSurec item in prj.KullaniciSurecs.Where(x => x.SurecId == surecID))
                {
                    syc++;
                    orn = orn + (float)item.IsTakibi.TamamlanmaOranı;

                }
                orn = orn / syc;
                Surec src = db.Surecs.Find(surecID);
                src.Tamamlanan =(int)orn;
                db.Entry(src).State = System.Data.Entity.EntityState.Modified;
                prj.SaveChanges();
            }
        }

        public void KsurecEkle(string AdSoyad, string RolAdi, int surecID, DateTime bslngc, DateTime bts, DateTime tmTarihi, int tmOrani)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                KullaniciSurec kl = new KullaniciSurec();
                kl.SurecId = surecID;
                kl.KullaniciId = db.Kullanicis.Where(x => x.Adsoyad == AdSoyad).FirstOrDefault().Id;
                kl.RolId = db.Rols.Where(x => x.RolAdi == RolAdi).FirstOrDefault().Id;
                kl.IstakibiId = isEkle(bslngc, bts, tmTarihi, tmOrani);
                prj.KullaniciSurecs.Add(kl);
                prj.SaveChanges();
                SurecOranHesapla(surecID);
            }
        }
        public int isEkle(DateTime bslngc, DateTime bts, DateTime tmTarihi, int tmOrani)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                IsTakibi istkb = new IsTakibi();
                istkb.BaslangicTarihi = bslngc;
                istkb.BitisTarihi = bts;
                istkb.TamamlanmaTarihi = tmTarihi;
                istkb.TamamlanmaOranı = OranKontrol(tmOrani);
                prj.IsTakibis.Add(istkb);
                prj.SaveChanges();
                int id = istkb.IstakibiId;
                return id;
            }
        }

        public void KsurecDuzenle(string AdSoyad, string RolAdi, int surecID, DateTime bslngc, DateTime bts, DateTime tmTarihi, int tmOrani)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                KullaniciSurec kl = db.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == RolAdi).FirstOrDefault();
                isDuzenle(bslngc, bts, tmTarihi, tmOrani, kl.IstakibiId);
                db.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                prj.SaveChanges();
                SurecOranHesapla(surecID);
            }
        }
        public void isDuzenle(DateTime bslngc, DateTime bts, DateTime tmTarihi, int tmOrani, int? isTakibiID)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                IsTakibi guncelIstakibi = db.IsTakibis.Find(isTakibiID);
                guncelIstakibi.BaslangicTarihi = bslngc;
                guncelIstakibi.BitisTarihi = bts;
                guncelIstakibi.TamamlanmaTarihi = tmTarihi;
                guncelIstakibi.TamamlanmaOranı = OranKontrol(tmOrani);
                db.Entry(guncelIstakibi).State = System.Data.Entity.EntityState.Modified;
                prj.SaveChanges();
            }

        }

        public int KullaniciSurecEkle(SurecIstakibi istakibi, int surecID)
        {
            bool isAnaliz = false, isTable = false, isProcedure = false, isDllList = false, isDllIslem = false, isArayuz = false, isTest = false;
            foreach (KullaniciSurec item in db.KullaniciSurecs.Where(x => x.SurecId == surecID))
            {

                if (item.Rol.RolAdi == "Analiz")
                {
                    KsurecDuzenle(istakibi.analizKisi, "Analiz", surecID, istakibi.analizBaslangicTarihi, istakibi.analizBitisTarihi, istakibi.analizTamamlanmaTarihi, istakibi.analizTamamlanmaOrani);
                    isAnaliz = true;
                    continue;

                }
                else if (item.Rol.RolAdi == "Table")
                {
                    isTable = true;
                    KsurecDuzenle(istakibi.tableKisi, "Table", surecID, istakibi.tableBaslangicTarihi, istakibi.tableBitisTarihi, istakibi.tableTamamlanmaTarihi, istakibi.tableTamamlanmaOrani);
                    continue;

                }
                else if (item.Rol.RolAdi == "Procedure")
                {
                    isProcedure = true;
                    KsurecDuzenle(istakibi.procedureKisi, "Procedure", surecID, istakibi.procedureBaslangicTarihi, istakibi.procedureBitisTarihi, istakibi.procedureTamamlanmaTarihi, istakibi.procedureTamamlanmaOrani);
                    continue;

                }
                else if (item.Rol.RolAdi == "DLL List")
                {
                    isDllList = true;
                    KsurecDuzenle(istakibi.dllListKisi, "DLL List", surecID, istakibi.dllListBaslangicTarihi, istakibi.dllListBitisTarihi, istakibi.dllListTamamlanmaTarihi, istakibi.dllListTamamlanmaOrani);
                    continue;


                }
                else if (item.Rol.RolAdi == "DLL Islem")
                {
                    isDllIslem = true;
                    KsurecDuzenle(istakibi.dllIslemKisi, "DLL Islem", surecID, istakibi.dllIslemBaslangicTarihi, istakibi.dllIslemBitisTarihi, istakibi.dllIslemTamamlanmaTarihi, istakibi.dllIslemTamamlanmaOrani);
                    continue;

                }
                else if (item.Rol.RolAdi == "Arayüz")
                {
                    isArayuz = true;
                    KsurecDuzenle(istakibi.arayuzKisi, "Arayüz", surecID, istakibi.arayuzBaslangicTarihi, istakibi.arayuzBitisTarihi, istakibi.arayuzTamamlanmaTarihi, istakibi.arayuzTamamlanmaOrani);
                    continue;


                }
                else if (item.Rol.RolAdi == "Test")
                {
                    isTest = true;
                    KsurecDuzenle(istakibi.testKisi, "Test", surecID, istakibi.testBaslangicTarihi, istakibi.testBitisTarihi, istakibi.testTamamlanmaTarihi, istakibi.testTamamlanmaOrani);
                    continue;
                }



            }
            DateTime dt = Convert.ToDateTime("01/01/2000");
            for (int i = 0; i < 7; i++)
            {


                if (isAnaliz == false)
                {
                    isAnaliz = true;
                    if (istakibi.analizBaslangicTarihi >dt && istakibi.analizBitisTarihi >dt && istakibi.analizTamamlanmaTarihi >dt)
                    {
                        KsurecEkle(istakibi.analizKisi, "Analiz", surecID, istakibi.analizBaslangicTarihi, istakibi.analizBitisTarihi, istakibi.analizTamamlanmaTarihi, istakibi.analizTamamlanmaOrani);
                    }

                }
                else if (isTable == false)
                {
                    isTable = true;
                    if (istakibi.tableBaslangicTarihi >dt && istakibi.tableBitisTarihi >dt && istakibi.tableTamamlanmaTarihi >dt)
                    {
                        KsurecEkle(istakibi.tableKisi, "Table", surecID, istakibi.tableBaslangicTarihi, istakibi.tableBitisTarihi, istakibi.tableTamamlanmaTarihi, istakibi.tableTamamlanmaOrani);
                    }

                }
                else if (isProcedure == false)
                {
                    isProcedure = true;
                    if (istakibi.procedureBaslangicTarihi >dt && istakibi.procedureBitisTarihi >dt && istakibi.procedureTamamlanmaTarihi >dt)
                    {
                        KsurecEkle(istakibi.procedureKisi, "Procedure", surecID, istakibi.procedureBaslangicTarihi, istakibi.procedureBitisTarihi, istakibi.procedureTamamlanmaTarihi, istakibi.procedureTamamlanmaOrani);
                    }

                }
                else if (isDllList == false)
                {
                    isDllList = true;
                    if (istakibi.dllListBaslangicTarihi >dt && istakibi.dllListBitisTarihi >dt && istakibi.dllListTamamlanmaTarihi >dt)
                    {
                        KsurecEkle(istakibi.dllListKisi, "DLL List", surecID, istakibi.dllListBaslangicTarihi, istakibi.dllListBitisTarihi, istakibi.dllListTamamlanmaTarihi, istakibi.dllListTamamlanmaOrani);
                    }

                }
                else if (isDllIslem == false)
                {
                    isDllIslem = true;
                    if (istakibi.dllIslemBaslangicTarihi >dt && istakibi.dllIslemBitisTarihi >dt && istakibi.dllIslemTamamlanmaTarihi >dt)
                    {
                        KsurecEkle(istakibi.dllIslemKisi, "DLL Islem", surecID, istakibi.dllIslemBaslangicTarihi, istakibi.dllIslemBitisTarihi, istakibi.dllIslemTamamlanmaTarihi, istakibi.dllIslemTamamlanmaOrani);
                    }

                }
                else if (isArayuz == false)
                {
                    isArayuz = true;
                    if (istakibi.arayuzBaslangicTarihi >dt && istakibi.arayuzBitisTarihi >dt && istakibi.arayuzTamamlanmaTarihi >dt)
                    {
                        KsurecEkle(istakibi.arayuzKisi, "Arayüz", surecID, istakibi.arayuzBaslangicTarihi, istakibi.arayuzBitisTarihi, istakibi.arayuzTamamlanmaTarihi, istakibi.arayuzTamamlanmaOrani);
                    }

                }
                else if (isTest == false)
                {
                    isTest = true;
                    if (istakibi.testBaslangicTarihi >dt && istakibi.testBitisTarihi >dt && istakibi.testTamamlanmaTarihi != null)
                    {
                        KsurecEkle(istakibi.testKisi, "Test", surecID, istakibi.testBaslangicTarihi, istakibi.testBitisTarihi, istakibi.testTamamlanmaTarihi, istakibi.testTamamlanmaOrani);
                    }

                }
                else
                {
                    break;
                }
            }
            db.SaveChanges();
            return 1;
        }

    }
}