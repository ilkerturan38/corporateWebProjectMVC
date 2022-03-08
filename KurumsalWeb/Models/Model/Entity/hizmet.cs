using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("hizmet")]
    public class hizmet
    {
        [Key]
        public int hizmetID { get; set; }

        [DisplayName("Hizmet Başlığı")]
        [Required, StringLength(150, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string baslik { get; set; }

        [Required, StringLength(500, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string aciklama { get; set; }

        public string resimURL { get; set; }
    }
}