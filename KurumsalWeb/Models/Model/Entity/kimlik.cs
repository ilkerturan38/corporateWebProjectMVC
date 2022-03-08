using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("kimlik")]
    public class kimlik
    {
        [Key]
        public int kimlikID { get; set; }

        [DisplayName("Site Başlığı")]
        [Required, StringLength(100, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string title { get; set; }

        [DisplayName("Anahtar Kelimeler")]
        [Required, StringLength(200, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string keywords { get; set; }

        [DisplayName("Site Açıklaması")]
        [Required, StringLength(300, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string description { get; set; }

        [DisplayName("Site Logosu")]
        public string logoURL { get; set; }

        [DisplayName("Site Ünvanı")]
        public string unvan { get; set; }
    }
}