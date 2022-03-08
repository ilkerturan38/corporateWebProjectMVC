using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("yorum")]
    public class yorum
    {
        [Key]
        public int yorumID { get; set; }

        [Required, StringLength(30, ErrorMessage = "30 Karakterden fazla değer giremessiniz..")]
        public string adsoyad { get; set; }
        public string email { get; set; }

        [DisplayName("Yorumunuz : ")]
        public string Icerik { get; set; }
        public bool yorumOnay { get; set; }
        public int? blogID { get; set; }
        public blog blog { get; set; }
    }
}