namespace YPYA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KullaniciSurec")]
    public partial class KullaniciSurec
    {
        public int Id { get; set; }

        public int? KullaniciId { get; set; }

        public int? SurecId { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Surec Surec { get; set; }
    }
}
