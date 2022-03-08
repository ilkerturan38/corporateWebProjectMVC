using KurumsalWeb.Models.Model.Context;
using KurumsalWeb.Models.Model.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class _HomeController : Controller
    {
        kurumsalDBContext db = new kurumsalDBContext();

        // SeoURL uyumlu olmasını istediğimiz,Her ActionResult üzerine Route ekliyoruz.
        // Index ActionResult Sayfamız Hem Anasayfa olarak, hemde _Home/Index olarak çalışıcak.

        [Route("Anasayfa")]
        //[Route("_Home/Index")]
        public ActionResult Index()
        {
            ViewBag.kimlik = db.kimliks.SingleOrDefault();
            ViewBag.Hizmet = db.hizmets.OrderBy(x => x.hizmetID).ToList();
            return View();
        }

        public ActionResult SliderPV()
        {
            var sorgu = db.sliders.OrderByDescending(x => x.ID).ToList();
            return PartialView(sorgu);
        }

        public ActionResult HizmetPV() // Index Sayfasında gösterilcek olan Hizmet PartialView Alanı
        {
            var sorgu = db.hizmets.ToList();
            return PartialView(sorgu);
        }

        [Route("Hizmetler")]
        public ActionResult Hizmet() // Hizmet Sayfası Alanı
        {
            return View();
        }

       
        public ActionResult FooterPV() // Template Sayfasında Tanımlandı.
        {
            ViewBag.Hizmet = db.hizmets.OrderBy(x => x.hizmetID).ToList();
            ViewBag.Iletisim = db.iletisims.SingleOrDefault();
            ViewBag.blog = db.blogs.OrderBy(x => x.blogID).ToList();
            return PartialView();
        }

        // Sadece Index sayfasında Tanımlarsa Kimlik Bilgileri,Diğer Sayfalara geçiş yapıldığında hata veriyor.Partial View yapıp Template Sayfasında tanımladık.
        public ActionResult kimlikPV() 
        {
            var sorgu = db.kimliks.SingleOrDefault();
            ViewBag.kimlik = sorgu;
            return PartialView();
        }

        public ActionResult siteLogoPV() // Template Sayfasında Tanımlandı.
        {
            var sorgu = db.kimliks.SingleOrDefault();
            ViewBag.kimlik = sorgu;
            return PartialView();
        }

        [Route("Hakkimizda")]
        public ActionResult Hakkimizda()
        {
            return View(db.hakkimizdas.SingleOrDefault());
        }


        [Route("blog/")] // {page?}
        public ActionResult Blog(int? page)
        {
            var sorgu = db.blogs.OrderBy(x => x.blogID).ToPagedList(page ?? 1, 3);
            return View(sorgu);

        }

        // SeoURL uyumlu

        [Route("blog/{baslik}-{id:int}")] // blogDetay sayfasına SeoURL tanımlaması yaptık.
        public ActionResult BlogDetay(int ID)
        {
            ViewBag.blogID = ID;
            var sorgu = db.blogs.Where(x => x.blogID == ID).SingleOrDefault();
            return View(sorgu);
        }

        // SeoURL uyumsuz QueryString'li İşlem

        //public ActionResult BlogDetay()
        //{
        //    int gelenID=int.Parse(Request.QueryString["ID"]);
        //    ViewBag.blogID = gelenID;
        //    var sorgu = db.blogs.Where(x => x.blogID == gelenID).SingleOrDefault();
        //    return View(sorgu);
        //}

        public ActionResult kategoriBlog(int ID) 
        {
            var sorgu = db.blogs.Where(x => x.Kategoriler.kategoriID == ID).ToList();
            return View(sorgu);
        }

        [Route("blog/{kategoriAdi}/{id:int}/{page?}")]
        public ActionResult kategoriBlog(int ID,int? page) // Blog Tablosundaki Kayıtların Parametreden Gelen Kategori ID'ye göre Listelenmesi
        {
           
            var sorgu = db.blogs.Where(x => x.Kategoriler.kategoriID == ID).ToList().ToPagedList(page ?? 1, 3);  // Parametreden gelen ID eşitse Blog tablosundaki Kategori ID'ye o Kategoriye ait kayıtları listele.
            return View(sorgu);
        }

        public ActionResult blogKategoriPV()
        {
            var sorgu = db.kategoriler.Include("blogs").OrderBy(x => x.kategoriAdi).ToList();
            return PartialView(sorgu);
        }

        
        public ActionResult BlogSonKayitlarPV()
        {
            var sorgu = db.blogs.OrderByDescending(x => x.blogID).ToList();
            return PartialView(sorgu);
        }

        public ActionResult yorumGetir(int ID) // ID'yi blogDetay sayfasına giden URL adresinden aldık.
        {
            db.Configuration.LazyLoadingEnabled = false;
            var sorgu = db.yorums.Include("blog").Where(x => x.blogID == ID && x.yorumOnay == true);  // Sadece onaylanmış (true) yorumları göster
            return PartialView(sorgu.ToList()); // ve URL Adresinden gelen BlogID,yorum Tablosundaki Blog ID'ye eşit olanı.Böylece Yorum'un hangi bloğa yazıldığı belli olur.
        }

        //public ActionResult yorumGetir()
        //{
        //    int gelenID = int.Parse(Request.QueryString["ID"]);
        //    var sorgu = db.yorums.Where(x => x.blogID == gelenID && x.yorumOnay == true).ToList();  // Sadece onaylanmış (true) yorumları göster
        //    return PartialView(sorgu); // ve URL Adresinden gelen BlogID,yorum Tablosundaki Blog ID'ye eşit olanı.Böylece Yorum'un hangi bloğa yazıldığı belli olur.
        //}

        [HttpGet]
        public ActionResult yorum()
        {
            return View();
        }

        [HttpPost] // Form'dan gelen İnputlara girilen verileri,parametre içerisinde değişken oluşturup(form içerisindeki,input'a verilen name adı ile aynı olmalı) verileri değişken içinde tutarak,işlemler yapabiliyoruz.
        public ActionResult yorum(yorum yeniYorum,string email) 
        {
            yeniYorum.email = email;
            db.yorums.Add(yeniYorum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult tiklanmaSayisi(int blogID)
        {
            var sorgu = db.blogs.Where(x => x.blogID == blogID).SingleOrDefault();
            sorgu.tiklanmaSayisi += 1; // gelen blogID'ye göre veritabanındaki blogID parametresinin,tiklanmaSayisini +1 arttırdık.
            db.SaveChanges();
            return View();
        }

        [Route("Iletisim")]
        public ActionResult Iletisim()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Iletisim(string adsoyad = null, string email = null, string konu = null, string mesaj = null) // Form'un Name alanından gelicek Parametreler
        {
            if (email != null && konu != null && mesaj != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "ayselturan038@gmail.com"; // Hangi Adrese Mesaj gidecek
                WebMail.Password = "Ucurtma38"; 
                WebMail.SmtpPort = 587;
                WebMail.Send("ilkerturan1728@gmail.com", konu, email + "-" + mesaj); // Alıcı,Konu,Gönderen,Mesaj
                ViewBag.uyari = "Mesajınız başarılı bir şekilde gönderilmiştir";
            }
            else
            {
                ViewBag.uyari = "Hata oluştu! Lütfen tekrar deneyiniz..";
            }

            return View();
        }

        [HttpGet]
        public ActionResult mailGonderme()
        {
            return View();
        }

        [HttpPost]
        public ActionResult mailGonderme(mail tst)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("ayselturan038@gmail.com"); // Alıcının Mail Adresi
                mail.From = new MailAddress(tst.email); // Gönderenin Mail Adresi
                mail.Subject = tst.konu;
                mail.Body = "Gönderen : " + tst.adsoyad + "<br/>" + "Mail Adresi :" + tst.email + "<br/>" + "Konu :" + tst.konu + "<br/>" + " Mesajı İçeriği :" + tst.mesaj;
                mail.IsBodyHtml = true; // Mesaj kısmında html özelliklerini kullanabilmek için ; true özelliği verdik.


                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential("ayselturan038@gmail.com", "Ucurtma38");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);
                    ViewBag.uyari = "Mail gönderilme işlemi başarı bir şekilde gerçekleşti. ";
                }
                catch (Exception ex)
                {
                    ViewBag.uyari = "Mail gönderilme işlemi sırasında hata oluştu! " + ex.Message;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult mailGonderme2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult mailGonderme2(string adsoyad, string email, string konu, string mesaj)
        {
            var fromAddress = "ayselturan038@gmail.com";
            const string fromPassword = "Ucurtma38";
            string subject = konu;
            string body = "Gönderen : " + adsoyad + "\n";
            body += "Email : " + email + "\n";
            body += "Konu : " + konu + "\n";
            body += "Mesaj : " + mesaj + "\n";

            // smtp settings
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 20000;
            }
            // Passing values to smtp object
            smtp.Send(email, "cumaerdgn38@gmail.com", subject, body); // gönderen-alici-konu-mesaj (Mesaj alındığında yukardaki fromAdress'ten gönderilmiş gibi gözüküyor)

            return View();
        }


        public ActionResult denemeRoute()
        {
            return View();
        }
    }
}