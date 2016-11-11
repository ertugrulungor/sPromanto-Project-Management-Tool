namespace YPYA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjeKullanici")]
    public partial class ProjeKullanici
    {
        public int Id { get; set; }

        public int? KullaniciId { get; set; }

        public int? ProjeId { get; set; }

        public int? RolId { get; set; }

        public DateTime? Tarih { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Proje Proje { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
