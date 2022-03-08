using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("kategoriler")]
    public class kategoriler
    {
        [Key]
        public int kategoriID { get; set; }
        public string kategoriAdi { get; set; }

        [Required, StringLength(500, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string kategoriAciklama { get; set; }

        public virtual List<blog> Blogs { get; set; }
    }
}