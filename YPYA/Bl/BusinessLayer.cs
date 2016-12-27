using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YPYA.Models;

namespace YPYA.Bl
{

    public class BusinessLayer
    {

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
                Surec src = prj.Surecs.Find(surecID);
                src.Tamamlanan = (int)orn;
                prj.Entry(src).State = System.Data.Entity.EntityState.Modified;
                prj.SaveChanges();   // en alt süreç oran hesaplaması yapıldı.
                SurecOranDuzenleme(surecID);


            }
        }
        public void SurecOranDuzenleme(int surecID)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                int parentSyc = 0;
                float? parentOran = 0;
                Surec Src = prj.Surecs.Find(surecID);
                int? parentID = Src.ParentSurecId; //Parentte arama yapabilmek için aldım.
                if (Src.ParentSurecId != null)
                {
                    foreach (Surec i in prj.Surecs.Where(x => x.ParentSurecId == Src.ParentSurecId))
                    {
                        parentSyc++;
                        parentOran = (float)parentOran + i.Tamamlanan;

                    }
                    parentOran = parentOran / parentSyc;
                    Surec parentSrc = prj.Surecs.Find(Src.ParentSurecId);
                    parentSrc.Tamamlanan = (int)parentOran;
                    prj.Entry(parentSrc).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges(); //parentta sureclerin oran hesaplamaları yapıldı.


                    parentOran = 0; parentSyc = 0;
                    Surec parent = prj.Surecs.Find(parentID);
                    if (parent.ParentSurecId != null)
                    {
                        foreach (Surec prnt in prj.Surecs.Where(x => x.ParentSurecId == parent.ParentSurecId)) 
                        {                            
                            parentSyc++;
                            parentOran = (float)parentOran + prnt.Tamamlanan;

                        }
                        parentOran = parentOran / parentSyc;
                        Surec BigParent = prj.Surecs.Find(parent.ParentSurecId);
                        BigParent.Tamamlanan = (int)parentOran;
                        prj.Entry(BigParent).State = System.Data.Entity.EntityState.Modified;
                        prj.SaveChanges(); 
                    }

                }                
            }
        }
        public void ProjeOranDuzenle(int ProjeID)
        {
            int projeSyc = 0;
            float? projeOran = 0;
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                foreach (Surec item in prj.Surecs.Where(x => x.ProjeId == ProjeID && x.ParentSurecId == null)) 
                {
                    projeSyc++;
                    projeOran = projeOran + item.Tamamlanan;                    

                }
                projeOran = projeOran / projeSyc;
                Proje pr = prj.Projes.Find(ProjeID);
                pr.Tamamlanan = (int)projeOran;
                prj.Entry(pr).State = System.Data.Entity.EntityState.Modified;
                prj.SaveChanges();
            }
        }

        public void KsurecEkle(string AdSoyad, string RolAdi, int surecID, DateTime bslngc, DateTime bts, DateTime tmTarihi, int tmOrani)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                KullaniciSurec kl = new KullaniciSurec();
                kl.SurecId = surecID;
                kl.KullaniciId = prj.Kullanicis.Where(x => x.Adsoyad == AdSoyad).FirstOrDefault().Id;
                kl.RolId = prj.Rols.Where(x => x.RolAdi == RolAdi).FirstOrDefault().Id;
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
                istkb.TamamlanmaOranı = tmOrani;
                if (tmTarihi != null)
                {
                    istkb.TamamlanmaTarihi = tmTarihi;
                }
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
                KullaniciSurec kl = prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == RolAdi).FirstOrDefault();
                kl.KullaniciId = prj.Kullanicis.Where(x => x.Adsoyad == AdSoyad).FirstOrDefault().Id;
                isDuzenle(bslngc, bts, tmTarihi, tmOrani, kl.IstakibiId);
                prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                prj.SaveChanges();
                SurecOranHesapla(surecID);

            }
        }
        public void isDuzenle(DateTime bslngc, DateTime bts, DateTime tmTarihi, int tmOrani, int? isTakibiID)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                IsTakibi guncelIstakibi = prj.IsTakibis.Find(isTakibiID);
                guncelIstakibi.BaslangicTarihi = bslngc;
                guncelIstakibi.BitisTarihi = bts;
                guncelIstakibi.TamamlanmaTarihi = tmTarihi;
                guncelIstakibi.TamamlanmaOranı = tmOrani;
                if (tmTarihi != null)
                {
                    guncelIstakibi.TamamlanmaTarihi = tmTarihi;
                }
                prj.Entry(guncelIstakibi).State = System.Data.Entity.EntityState.Modified;
                prj.SaveChanges();
            }

        }

        public int KullaniciSurecEkle(SurecIstakibi istakibi, int surecID,int projeID)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                if (istakibi.surecNote != null)
                {
                    Surec src = prj.Surecs.Find(surecID);
                    src.Note = istakibi.surecNote;
                    prj.SaveChanges();
                }
                bool isAnaliz = false, isTable = false, isProcedure = false, isDllList = false, isDllIslem = false, isArayuz = false, isTest = false;
                foreach (KullaniciSurec item in prj.KullaniciSurecs.Where(x => x.SurecId == surecID))
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
                        if (istakibi.analizBaslangicTarihi > dt && istakibi.analizBitisTarihi > dt)
                        {
                            KsurecEkle(istakibi.analizKisi, "Analiz", surecID, istakibi.analizBaslangicTarihi, istakibi.analizBitisTarihi, istakibi.analizTamamlanmaTarihi, istakibi.analizTamamlanmaOrani);
                        }

                    }
                    else if (isTable == false)
                    {
                        isTable = true;
                        if (istakibi.tableBaslangicTarihi > dt && istakibi.tableBitisTarihi > dt)
                        {
                            KsurecEkle(istakibi.tableKisi, "Table", surecID, istakibi.tableBaslangicTarihi, istakibi.tableBitisTarihi, istakibi.tableTamamlanmaTarihi, istakibi.tableTamamlanmaOrani);
                        }

                    }
                    else if (isProcedure == false)
                    {
                        isProcedure = true;
                        if (istakibi.procedureBaslangicTarihi > dt && istakibi.procedureBitisTarihi > dt)
                        {
                            KsurecEkle(istakibi.procedureKisi, "Procedure", surecID, istakibi.procedureBaslangicTarihi, istakibi.procedureBitisTarihi, istakibi.procedureTamamlanmaTarihi, istakibi.procedureTamamlanmaOrani);
                        }

                    }
                    else if (isDllList == false)
                    {
                        isDllList = true;
                        if (istakibi.dllListBaslangicTarihi > dt && istakibi.dllListBitisTarihi > dt)
                        {
                            KsurecEkle(istakibi.dllListKisi, "DLL List", surecID, istakibi.dllListBaslangicTarihi, istakibi.dllListBitisTarihi, istakibi.dllListTamamlanmaTarihi, istakibi.dllListTamamlanmaOrani);
                        }

                    }
                    else if (isDllIslem == false)
                    {
                        isDllIslem = true;
                        if (istakibi.dllIslemBaslangicTarihi > dt && istakibi.dllIslemBitisTarihi > dt)
                        {
                            KsurecEkle(istakibi.dllIslemKisi, "DLL Islem", surecID, istakibi.dllIslemBaslangicTarihi, istakibi.dllIslemBitisTarihi, istakibi.dllIslemTamamlanmaTarihi, istakibi.dllIslemTamamlanmaOrani);
                        }

                    }
                    else if (isArayuz == false)
                    {
                        isArayuz = true;
                        if (istakibi.arayuzBaslangicTarihi > dt && istakibi.arayuzBitisTarihi > dt )
                        {
                            KsurecEkle(istakibi.arayuzKisi, "Arayüz", surecID, istakibi.arayuzBaslangicTarihi, istakibi.arayuzBitisTarihi, istakibi.arayuzTamamlanmaTarihi, istakibi.arayuzTamamlanmaOrani);
                        }

                    }
                    else if (isTest == false)
                    {
                        isTest = true;
                        if (istakibi.testBaslangicTarihi > dt && istakibi.testBitisTarihi > dt)
                        {
                            KsurecEkle(istakibi.testKisi, "Test", surecID, istakibi.testBaslangicTarihi, istakibi.testBitisTarihi, istakibi.testTamamlanmaTarihi, istakibi.testTamamlanmaOrani);
                        }

                    }
                    else
                    {
                        break;
                    }
                }

                prj.SaveChanges();
                ProjeOranDuzenle(projeID);
                return 1;
            }
        }

        public int SurecBilgiKaydet(SurecIstakibi istakibiBilgi ,int surecID,int projeID)
        {    
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                
                if (istakibiBilgi.analizKisi != null)
                {
                   KullaniciSurec kl= prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == "Analiz").FirstOrDefault();
                    kl.IsTakibi.TamamlanmaOranı = istakibiBilgi.analizTamamlanmaOrani;
                    DateTime dt = Convert.ToDateTime("01/01/2000");
                    if (istakibiBilgi.analizTamamlanmaTarihi > dt)
                    {
                        kl.IsTakibi.TamamlanmaTarihi = istakibiBilgi.analizTamamlanmaTarihi;
                    }
                    prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges();
                    SurecOranHesapla(surecID);
                }

                if (istakibiBilgi.tableKisi != null) 
                {
                    KullaniciSurec kl = prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == "Table").FirstOrDefault();
                    kl.IsTakibi.TamamlanmaOranı = istakibiBilgi.tableTamamlanmaOrani;
                    DateTime dt = Convert.ToDateTime("01/01/2000");
                    if (istakibiBilgi.tableTamamlanmaTarihi > dt)
                    {
                        kl.IsTakibi.TamamlanmaTarihi = istakibiBilgi.tableTamamlanmaTarihi;
                    }
                    prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges();
                    SurecOranHesapla(surecID);
                }

                if (istakibiBilgi.procedureKisi != null)
                {
                    KullaniciSurec kl = prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == "Procedure").FirstOrDefault();
                    kl.IsTakibi.TamamlanmaOranı = istakibiBilgi.procedureTamamlanmaOrani;
                    DateTime dt = Convert.ToDateTime("01/01/2000");
                    if (istakibiBilgi.procedureTamamlanmaTarihi > dt)
                    {
                        kl.IsTakibi.TamamlanmaTarihi = istakibiBilgi.procedureTamamlanmaTarihi;
                    }
                    prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges();
                    SurecOranHesapla(surecID);
                }

                if (istakibiBilgi.dllListKisi != null)
                {
                    KullaniciSurec kl = prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == "DLL List").FirstOrDefault();
                    kl.IsTakibi.TamamlanmaOranı = istakibiBilgi.dllListTamamlanmaOrani;
                    DateTime dt = Convert.ToDateTime("01/01/2000");
                    if (istakibiBilgi.dllListTamamlanmaTarihi > dt)
                    {
                        kl.IsTakibi.TamamlanmaTarihi = istakibiBilgi.dllListTamamlanmaTarihi;
                    }
                    prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges();
                    SurecOranHesapla(surecID);

                }

                if (istakibiBilgi.dllIslemKisi != null)
                {
                    KullaniciSurec kl = prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == "DLL Islem").FirstOrDefault();
                    kl.IsTakibi.TamamlanmaOranı = istakibiBilgi.analizTamamlanmaOrani;
                    DateTime dt = Convert.ToDateTime("01/01/2000");
                    if (istakibiBilgi.dllIslemTamamlanmaTarihi > dt)
                    {
                        kl.IsTakibi.TamamlanmaTarihi = istakibiBilgi.dllIslemTamamlanmaTarihi;
                    }
                    prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges();
                    SurecOranHesapla(surecID);

                }

                if (istakibiBilgi.arayuzKisi != null)
                {
                    KullaniciSurec kl = prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == "Arayüz").FirstOrDefault();
                    kl.IsTakibi.TamamlanmaOranı = istakibiBilgi.arayuzTamamlanmaOrani;
                    DateTime dt = Convert.ToDateTime("01/01/2000");
                    if (istakibiBilgi.analizTamamlanmaTarihi > dt)
                    {
                        kl.IsTakibi.TamamlanmaTarihi = istakibiBilgi.arayuzTamamlanmaTarihi;
                    }
                    prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges();
                    SurecOranHesapla(surecID);
                }

                if (istakibiBilgi.testKisi != null)
                {
                    KullaniciSurec kl = prj.KullaniciSurecs.Where(x => x.SurecId == surecID && x.Rol.RolAdi == "Test").FirstOrDefault();
                    kl.IsTakibi.TamamlanmaOranı = istakibiBilgi.testTamamlanmaOrani;
                    DateTime dt = Convert.ToDateTime("01/01/2000");
                    if (istakibiBilgi.analizTamamlanmaTarihi > dt)
                    {
                        kl.IsTakibi.TamamlanmaTarihi = istakibiBilgi.testTamamlanmaTarihi;
                    }
                    prj.Entry(kl).State = System.Data.Entity.EntityState.Modified;
                    prj.SaveChanges();
                    SurecOranHesapla(surecID);
                }

                ProjeOranDuzenle(projeID);
                return 1;
            }
        }

        public int SurecSilme(int surecID)
        {
            using (projeyonetimvtEntities prj = new projeyonetimvtEntities())
            {
                Surec src = prj.Surecs.Find(surecID);
                if (src.ParentSurecId != null)
                {
                    foreach (Surec item in prj.Surecs.Where(x => x.ParentSurecId == surecID))
                    {
                        if (item.ParentSurecId != null)
                        {
                            foreach (Surec i in prj.Surecs.Where(x => x.ParentSurecId == item.Id)) 
                            {
                                if (i.ParentSurecId != null)
                                {
                                    foreach (KullaniciSurec kl in prj.KullaniciSurecs.Where(x=>x.SurecId == i.Id))
                                    {
                                        prj.KullaniciSurecs.Remove(kl);

                                    }
                                    prj.Surecs.Remove(i);
                                }

                            }
                            foreach (KullaniciSurec kl in prj.KullaniciSurecs.Where(x=>x.SurecId == item.Id))
                            {
                                prj.KullaniciSurecs.Remove(kl);
                            }
                            prj.Surecs.Remove(item);
                        }

                    } 
                    foreach (KullaniciSurec kl in prj.KullaniciSurecs.Where(x => x.SurecId == src.Id))
                    {
                        prj.KullaniciSurecs.Remove(kl);
                    }
                    prj.Surecs.Remove(src);
                }
                else
                {
                    foreach (KullaniciSurec kl in prj.KullaniciSurecs.Where(x => x.SurecId == src.Id))
                    {
                        prj.KullaniciSurecs.Remove(kl);
                    }
                    prj.Surecs.Remove(src);
                }       

                prj.SaveChanges();
                return 1;
            }
        }
    }
}