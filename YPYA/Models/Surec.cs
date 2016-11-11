namespace YPYA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Surec")]
    public partial class Surec
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Surec()
        {
            KullaniciSurecs = new HashSet<KullaniciSurec>();
            Surec1 = new HashSet<Surec>();
        }

        public int Id { get; set; }

        [StringLength(70)]
        public string Baslik { get; set; }

        public DateTime? PlanBaslangic { get; set; }

        public DateTime? PlanBitis { get; set; }

        public DateTime? GercekBaslangic { get; set; }

        public DateTime? GercekBitis { get; set; }

        public int? Tamamlanan { get; set; }

        public int? OlusturanId { get; set; }

        public int? DurumId { get; set; }

        public int? OncelikId { get; set; }

        public int? AltSurec { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public virtual Durum Durum { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KullaniciSurec> KullaniciSurecs { get; set; }

        public virtual Oncelik Oncelik { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Surec> Surec1 { get; set; }

        public virtual Surec Surec2 { get; set; }
    }
}
