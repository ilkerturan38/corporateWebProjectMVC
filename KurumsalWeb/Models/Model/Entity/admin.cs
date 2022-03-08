using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("admin")]
    public class admin
    {
        [Key]
        public int adminID { get; set; }

        [Required,StringLength(50,ErrorMessage ="Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string adminMail { get; set; }

        [Required, StringLength(50, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string adminPassword { get; set; }

        public string yetki { get; set; }

        public virtual bool beniHatirla { get; set; }

        
    }
}