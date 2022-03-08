using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    public class mail
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string adsoyad { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string konu { get; set; }

        [Required]
        public string mesaj { get; set; }
        
    }
}