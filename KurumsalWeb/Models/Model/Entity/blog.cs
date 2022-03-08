using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("blog")]
    public class blog
    {
        [Key]
        public int blogID { get; set; }

        [DisplayName("Blok Başlığı")]
        public string Baslik { get; set; }

        [DisplayName("Blok İçeriği")]
        public string Icerik { get; set; }

        [DisplayName("Blok Resmi")]
        public string resimURL { get; set; }

        public int? kategoriID { get; set; }
        public virtual kategoriler Kategoriler { get; set; }

        public ICollection<yorum> yorums { get; set; }

        public int tiklanmaSayisi { get; set; }
    }
}