using KurumsalWeb.Models.Model.Context;
using KurumsalWeb.Models.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace KurumsalWeb.Controllers
{
    public class _AdminController : Controller
    {
        kurumsalDBContext db = new kurumsalDBContext();


        [Route("yonetimpaneli")]
        public ActionResult Index()
        {
            ViewBag.blogSayisi = db.blogs.Count();
            ViewBag.hizmetSayisi = db.hizmets.Count();
            ViewBag.yorumSayisi = db.yorums.Count();
            ViewBag.kategoriSayisi = db.kategoriler.Count();
            return View();
        }

        [Route("yonetimpaneli/login")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [Route("yonetimpaneli/login")]
        [HttpPost]
        public ActionResult Login(admin adm, string adminPassword)
        {
            var passw = Crypto.Hash(adm.adminPassword, "MD5"); // Login Kısmında Girilen şifreyi MD5'e çevir ve Veritabanındaki MD5 ile karşılaştır.Aynı ise girişe izin ver.
            var sorgu = db.admins.FirstOrDefault(x => x.adminMail == adm.adminMail && x.adminPassword == passw);
            if (sorgu != null)
            {
                // ********************** Session Kullanımı **********************

                //Session["ID"] = sorgu.adminID;
                //Session["kAdi"] = sorgu.adminMail.ToString();
                //Session["kSifre"] = sorgu.adminPassword.ToString();

                // ********************** Session Kullanımı **********************


                // ********************** Cookie Kullanımı **********************

                // Cookie Kullanımı 1
                
                HttpCookie cerez = new HttpCookie("admin");
                cerez["kAdi"] = sorgu.adminMail.ToString();
                cerez["kSifre"] = sorgu.adminPassword.ToString();
                cerez["yetki"] = sorgu.yetki.ToString();
                cerez.Expires = DateTime.Now.AddDays(1); // Son kullanım Tarihi ; Şuanki tarihe 1 gün ekle
                Response.Cookies.Add(cerez);

                // Cookie Kullanımı 2

                Response.Cookies["ozel"]["ID"] = sorgu.adminID.ToString(); // Üye Girişi yapmış olan Kullanıcının ID Bilgisini Cookie'ye atıp,Profil Sayfasında Sadece Giriş yapmış kullanıcının ID'sini,Veritabanındaki ID ile karşılaştırıp bilgilerini gösterdik .
                Response.Cookies["ozel"].Expires = DateTime.Now.AddDays(1);

                // ********************** Cookie Kullanımı **********************

                return RedirectToAction("Index", "_Admin"); // Controller içerisindeki Index'e gidiyor fakat,Index Linkini SeoURL yaptığımız için,verilen linke göre açıldı.
            }
            ViewBag.hata = "Kullanıcı Adı veya Şifre Hatalı";
            return View();

        }

        [Route("yonetimpaneli/singout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "_Admin");

            // Session Sonlandırma işlemi (Eğer Hiçbir işlem yapılmassa Sitede,WebConfing Sayfasında tanımlanan sessionState yöntemi ile 20 dakika sonra hesap bilgilerini tekrar istiycek.)

            //Session["kAdi"] = null;
            //Session["kSifre"] = null;
            //Session["ID"] = null;
            //Session.Abandon();
        }

        [Route("sifremiUnuttum")]
        [HttpGet]
        public ActionResult sifremiUnuttum()
        {
            return View();
        }


        [Route("sifremiUnuttum")]
        [HttpPost]
        public ActionResult sifremiUnuttum(string email)
        {
            var sorgu = db.admins.Where(x => x.adminMail == email).SingleOrDefault();
            if (sorgu != null)
            {
                Random rnd = new Random();
                int yeniSifre = rnd.Next();

                sorgu.adminPassword= Crypto.Hash(Convert.ToString(yeniSifre), "MD5");
                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "ayselturan038@gmail.com"; // Web Sitemizde,Mail trafiğini yöneticek Ana Mail Adresi
                WebMail.Password = "Ucurtma38";
                WebMail.SmtpPort = 587;
                WebMail.Send(email, "Kurumsal Web"," Yeni Şifreniz => " + yeniSifre); // Alıcı,Konu,Mesaj
                ViewBag.uyari = "Yeni Şifreniz Mail adresinize başarılı bir şekilde gönderilmiştir..";
            }
            else
            {
                ViewBag.uyari = "Hata oluştu! Şifre Yenileme İstediğiniz Gerçekleşemedi.Lütfen tekrar deneyiniz..";
            }
            return View();
        }


        [Route("yonetimpaneli/profil")]
        [HttpGet]
        public ActionResult Profil()
        {
            // ********************** Session Kullanımı **********************

            //var islem = Session["ID"];
            //var sorgu = db.admins.SingleOrDefault(x => (int)x.adminID == (int)islem);

            // ********************** Session Kullanımı **********************


            // ********************** Cookie Kullanımı **********************

            var deger = Request.Cookies["ozel"]["ID"]; // Login Controller'da oluşturulan Cookie'yi alıp,kullanabilmek için Request Yöntemi kullanıldı.
            var sorgu = db.admins.SingleOrDefault(x => x.adminID.ToString() == deger); // (int) değer karşılaştırırken Hem Yeni Oluşturulan Cookie'ye hemde veritabanındaki ID değerine ToString ifadesini eklenmeli.. ;  

            // ********************** Cookie Kullanımı **********************

            return View(sorgu);
        }

        [Route("yonetimpaneli/profilGuncelle")]
        [HttpGet]
        public ActionResult profilGuncelle()
        {
            var deger = Request.Cookies["ozel"]["ID"];
            var sorgu = db.admins.SingleOrDefault(x => x.adminID.ToString() == deger);
            return View(sorgu);
        }


        [Route("yonetimpaneli/profilGuncelle")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profilGuncelle(admin adm)
        {

            try
            {
                var deger = Request.Cookies["ozel"]["ID"];
                var sorgu = db.admins.SingleOrDefault(x => x.adminID.ToString() == deger);
                sorgu.adminMail = adm.adminMail;
                sorgu.adminPassword = Crypto.Hash(adm.adminPassword, "MD5"); // Veritabanına Şifreyi MD5(Korumalı) olarak kaydet
                sorgu.yetki = adm.yetki;
                db.SaveChanges();
                ViewBag.hata = "Güncelle işlemi başarılı bir şekilde gerçekleşti..";
                return RedirectToAction("Profil", "_Admin");

            }
            catch (Exception ex)
            {
                ViewBag.hata = "Güncelle işlemi sırasında hata meydana geldi!" + ex;
            }
            return View();
        }
    }
}