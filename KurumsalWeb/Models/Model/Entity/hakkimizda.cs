using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model.Entity
{
    [Table("hakkimizda")] // Tablo ismi vermek için kullanılır.
    public class hakkimizda
    {
        [Key]
        public int hakkimizdaID { get; set; }

        [DisplayName("Açıklama")] // Bilgilendirme Metni olarak kullanılır.
        [Required]
        [StringLength(100, ErrorMessage = "Belirtilen Karakter Sınırlamasını Aştınız..")]
        public string aciklama { get; set; }
    }
}