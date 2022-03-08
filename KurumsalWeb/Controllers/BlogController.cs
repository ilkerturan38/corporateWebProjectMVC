using KurumsalWeb.Models.Model.Context;
using KurumsalWeb.Models.Model.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        kurumsalDBContext db = new kurumsalDBContext();
        
        [Route("adminBlog")]
        public ActionResult Index()
        {
            /*
                Foreign Key : Blog Tablosundaki kategoriID, Kategoriler Tablosundaki kategoriID ile bağlantılı.
                Bir Tabloyu Listelerken Foreign Bağlantısı var ise Tabloda ; Include ile Bağlantılı olduğu Tabloyuda Ekleyip O tabloya ait olan kayıtları çekebilicez.
            */

            db.Configuration.LazyLoadingEnabled = false;
            return View(db.blogs.Include("kategoriler").ToList());  
        }

        [Route("adminYeniBlogEkle")]
        public ActionResult Create()
        {
            // Listeden Bir değer Seç ; 
            List<SelectListItem> icerik = (from item in db.kategoriler.ToList() // Kategoriler tablosunun bütün değerlerini al
                                           select new SelectListItem // yukarıdaki liste öğesini seç
                                           {
                                               // DropdownList'in aldığı parametreler
                                               Text = item.kategoriAdi, // Seçilen değerin İçeriği
                                               Value = item.kategoriID.ToString() // Seçilen değerin ID'si
                                           }).ToList();
            ViewBag.kategoriID = icerik;
            return View();
        }

        [Route("adminYeniBlogEkle")]
        [HttpPost]
        [ValidateInput(false)] // Zararlı olabilecek bir Request.Form Algılandı => Hatasının Çözümü 
        [ValidateAntiForgeryToken] // Güvenlik Amaçlı
        public ActionResult Create(blog yeniBlog, HttpPostedFileBase resimURL)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ktg = db.kategoriler.Where(p => p.kategoriID == yeniBlog.kategoriID).FirstOrDefault();
                    yeniBlog.Kategoriler = ktg;
                    FileInfo file = new FileInfo(resimURL.FileName);
                    int iFileSize = resimURL.ContentLength;
                    if (iFileSize < (1024 * 1024 * 5))
                    {
                        var dosyayoluKontrol = System.IO.Path.Combine(Server.MapPath("~/Uploads/blog/") + file); // Proje içerisindeki bulunan blog Klasörüne kaydeder.

                        resimURL.SaveAs(dosyayoluKontrol);
                        yeniBlog.resimURL = "/Uploads/blog/" + file; // Veritabanına kaydeder.
                        db.blogs.Add(yeniBlog);
                        db.SaveChanges();
                        TempData["uyari"] = "Yeni Blog Kaydı ekleme İşlemi başarılı bir şekilde gerçekleşti!";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Error = "Resim yükleme boyutu Maksimum 5 MB olmalıdır.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Yeni Blog Kaydı ekleme işlemi sırasında hata meydana geldi!" + ex;
            }
            return View(yeniBlog);
        }

        
        [HttpGet]
        [Route("adminBlogGuncelle/{id:int}")]
        public ActionResult update(int ID)
        {

            if (ID == null)
            {
                return HttpNotFound();
            }
            var islem = db.blogs.Where(x => x.blogID == ID).SingleOrDefault();
            if (islem == null)
            {
                return HttpNotFound();
            }
            ViewBag.kategoriID = new SelectList(db.kategoriler, "kategoriID", "kategoriAdi", islem.kategoriID);

            return View(islem);
        }

        [Route("adminBlogGuncelle/{id:int}")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult update(int ID ,blog mevcutBlog, HttpPostedFileBase resimURL) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var sorgu = db.blogs.Where(x => x.blogID == ID).SingleOrDefault();
                    var sorgu = db.blogs.Find(ID);
                    int iFileSize = resimURL.ContentLength;
                    FileInfo file = new FileInfo(resimURL.FileName);

                    if (iFileSize < (1024 * 1024 * 5))
                    {
                        var dosyayoluKontrol = System.IO.Path.Combine(Server.MapPath("~/Uploads/blog/") + file);

                        sorgu.Baslik = mevcutBlog.Baslik;
                        sorgu.Icerik = mevcutBlog.Icerik;
                        sorgu.resimURL = mevcutBlog.resimURL;
                        sorgu.kategoriID = mevcutBlog.kategoriID;
                        resimURL.SaveAs(dosyayoluKontrol);
                        sorgu.resimURL = "/Uploads/blog/" + file;
                        db.SaveChanges();
                        TempData["uyari"] = "Güncelleme İşlemi başarılı bir şekilde gerçekleşti!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["uyari"] = "Dosya boyutunuz Maksimum 5 MB olmalıdır.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["uyari"] = "Güncelleme işlemi sırasında hata meydana geldi!" + ex;
            }
            return View();
        }

        [Route("adminBlogSil/{id:int}")]
        public ActionResult deleted(int ID)
        {
            try
            {
                var sorgu = db.blogs.Find(ID);
                db.blogs.Remove(sorgu);
                db.SaveChanges();
                TempData["uyari"] = "Dosya Silme İşlemi başarılı bir şekilde gerçekleşti!";
                
            }
            catch(Exception ex)
            {
                TempData["uyari"] = "Dosya silme işlemi sırasında hata meydana geldi!" + ex;
            }
            return RedirectToAction("Index");
        }
    }
}
