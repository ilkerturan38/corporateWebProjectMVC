using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("iletisim")]
    public class iletisim
    {
        [Key]
        public int iletisimID { get; set; }

        [Required, StringLength(500, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string adres { get; set; }
        public string tel { get; set; }
        public string mailAdres { get; set; }
        public string whatshapp { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string instagram { get; set; }
    }
}