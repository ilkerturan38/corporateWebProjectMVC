using KurumsalWeb.Models.Model.Context;
using KurumsalWeb.Models.Model.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HizmetController : Controller
    {
        kurumsalDBContext db = new kurumsalDBContext();
        // GET: Hizmet
        public ActionResult Index(int? sayfa) // Sayfalar arası geçiş yapabilmek için sayfa adında değişken tanımladık.Sayfa değeri boş gelebilir
        {

            return View(db.hizmets.ToList().ToPagedList(sayfa??1,4)); // Boş gelirse Enbaştan listele,Dolu Gelirse gelen Sayfa Numarasına göre,Her Sayfada Kaç Veri gösterilceğini belirle.
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(hizmet yeniHizmet, HttpPostedFileBase resimURL) // Parametre içerisindeki yeniHizmet referansı,Create Sayfasında Form içinde bulunan input,textarea vb. alanlara girilen yeni bilgileri tutuyor. 
           {
            try
            {
                if (ModelState.IsValid)
                {
                    int iFileSize = resimURL.ContentLength;
                    var dosyaAdi = Path.GetFileName(resimURL.FileName);

                    if (iFileSize < (1024 * 1024 * 5))
                    {
                        var dosyayoluKontrol = System.IO.Path.Combine(Server.MapPath("~/Uploads/hizmet/") + dosyaAdi);
                        if (!System.IO.File.Exists(dosyayoluKontrol))
                        {
                            resimURL.SaveAs(dosyayoluKontrol);
                            yeniHizmet.resimURL = "/Uploads/hizmet/" + dosyaAdi;

                            db.hizmets.Add(yeniHizmet);
                            db.SaveChanges();
                            TempData["uyari"] = "Yeni Hizmet yükleme İşlemi başarılı bir şekilde gerçekleşti!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["uyari"] = "Yüklemeye çalıştığınız dosya sistemde kayıtlıdır.\n Lütfen Farklı bir dosya yükleyiniz..";
                        }
                    }
                    else
                    {
                        TempData["uyari"] = "Resim yükleme boyutu Maksimum 5 MB olmalıdır.";
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["uyari"] = "Yeni Kayıt ekleme işlemi sırasında hata meydana geldi!" + ex;
            }
            return View(yeniHizmet);
        }

        [HttpGet]
        public ActionResult update(int ID) // Formlar arası veri taşıma işlemi
        {
            // Hizmetler tablosunda bulunan ID,Parametre olarak gelicek ID'ye eşit Mi ? 

            var sorgu = db.hizmets.Where(p => p.hizmetID == ID).FirstOrDefault();
            return View(sorgu);
        }

        [HttpPost]
        [ValidateInput(false)]

        // Update sayfasında Hizmet Sınıfından @model oluşturduk.Veritabanın bulunan mevcut kayıtları görebilmek ve yeni girilen değerleri hafızada tutabilmek için Inputlara Modeldeki alanları atadık.
        // Parametre içerisinde,update sayfası içinde tanımlanan hizmet modelinde tutulan,Input'lara yeni girilen kayıtları çekip,güncelleme işlemi yapabilmek için hizmet modelinden referans oluşturduk.
        public ActionResult update(hizmet hizmetGuncel, HttpPostedFileBase resimURL) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int iFileSize = resimURL.ContentLength;
                    FileInfo file = new FileInfo(resimURL.FileName);

                    if (iFileSize < (1024 * 1024 * 5))
                    {
                        var dosyayoluKontrol = System.IO.Path.Combine(Server.MapPath("~/Uploads/hizmet/") + file);

                        var sorgu = db.hizmets.Find(hizmetGuncel.hizmetID); // Sorgu = Gelen ID'ye ait Veritabanındaki Alanları Tutar -- 
                        sorgu.baslik = hizmetGuncel.baslik; // hizmetGuncel = Güncelleme işlemi için,Girilen yeni veya mevcut değerin bilgisini tutuyor.
                        sorgu.aciklama = hizmetGuncel.aciklama;

                        resimURL.SaveAs(dosyayoluKontrol);
                        sorgu.resimURL = "/Uploads/hizmet/" + file;

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
            catch(Exception ex)
            {
                TempData["uyari"] = "Güncelleme işlemi sırasında hata meydana geldi!"+ex;
            }
            return View(hizmetGuncel); // Hata olduğunda ; Var olan güncel değerler Bize geri dönsün.
        }

        
        public ActionResult delete(int ID)
        {
            try
            {
                var sorgu = db.hizmets.Find(ID); // Hizmet Tablosunda,URL ile gelen ID'ye ait Satırı bul
               
                db.hizmets.Remove(sorgu);
                db.SaveChanges();
                System.IO.File.Delete(Server.MapPath("~/Uploads/hizmet/"+ sorgu.resimURL.ToString()+".png"));
                TempData["uyari"] = "Silme işlemi başarı bir şekilde gerçekleşti.";
               
            }
            catch (Exception ex)
            {
                TempData["uyari"] = "Silme işlemi sırasında hata meydana geldi!" + ex;
            }
            return RedirectToAction("Index");
        }

    }
}
