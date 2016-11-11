namespace YPYA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Proje")]
    public partial class Proje
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proje()
        {
            MusteriIsteris = new HashSet<MusteriIsteri>();
            ProjeKullanicis = new HashSet<ProjeKullanici>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Baslik { get; set; }

        public int? KullaniciId { get; set; }

        public DateTime? PlanBaslangic { get; set; }

        public DateTime? PlanBitis { get; set; }

        public DateTime? GercekBaslangic { get; set; }

        public DateTime? GercekBitis { get; set; }

        public DateTime? Olusturulma { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MusteriIsteri> MusteriIsteris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjeKullanici> ProjeKullanicis { get; set; }
    }
}
