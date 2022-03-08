using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("Slider")]
    public class Slider
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50, ErrorMessage = "Maksimum 50 Karakter Girebilirsiniz..")]
        public string baslik { get; set; }

        [StringLength(300, ErrorMessage = "Maksimum 300 Karakter Girebilirsiniz..")]
        public string aciklama { get; set; }

        [Required]
        public string resimURL { get; set; }

    }
}