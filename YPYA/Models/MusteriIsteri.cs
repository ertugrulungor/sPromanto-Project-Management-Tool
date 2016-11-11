namespace YPYA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MusteriIsteri")]
    public partial class MusteriIsteri
    {
        public int Id { get; set; }

        public string Icerik { get; set; }

        public DateTime? Tarih { get; set; }

        public int? ProjeId { get; set; }

        public virtual Proje Proje { get; set; }
    }
}
