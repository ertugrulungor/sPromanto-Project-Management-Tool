namespace YPYA.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class yonetimDB : DbContext
    {
        public yonetimDB()
            : base("name=yonetimDB")
        {
        }

        public virtual DbSet<Durum> Durums { get; set; }
        public virtual DbSet<Kullanici> Kullanicis { get; set; }
        public virtual DbSet<KullaniciSurec> KullaniciSurecs { get; set; }
        public virtual DbSet<MusteriIsteri> MusteriIsteris { get; set; }
        public virtual DbSet<Oncelik> Onceliks { get; set; }
        public virtual DbSet<Proje> Projes { get; set; }
        public virtual DbSet<ProjeKullanici> ProjeKullanicis { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<Surec> Surecs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Surecs)
                .WithOptional(e => e.Kullanici)
                .HasForeignKey(e => e.OlusturanId);

            modelBuilder.Entity<Surec>()
                .HasMany(e => e.Surec1)
                .WithOptional(e => e.Surec2)
                .HasForeignKey(e => e.AltSurec);
        }
    }
}
