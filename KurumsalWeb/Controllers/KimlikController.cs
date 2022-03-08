using KurumsalWeb.Models.Model.Context;
using KurumsalWeb.Models.Model.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class KimlikController : Controller
    {
        kurumsalDBContext dbModel = new kurumsalDBContext();

        // GET: Kimlik
        public ActionResult Index()
        {
            return View(dbModel.kimliks.ToList());
        }

        // GET: Kimlik/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Kimlik/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kimlik/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)  // Veritabanındaki kayıtların Textbox vb. özelliklerde gösterilmesi aşaması
        {
            /*
               SingleOrDefault :  Belirtilen şarta göre Tek bir eleman döndürmek için kullanılır.Şartın sağlanmadığı durumda hata yerine o tipin varsayılan (0) değerini döndürür. 
               FirstOrDefault  :  Belirtilen şarta göre İlk elemanı döndürür.Şartın sağlanmadığı durumda hata yerine o tipin varsayılan (0) değerini döndürür.
            */

            var sorgu = dbModel.kimliks.Where(x => x.kimlikID == id).SingleOrDefault();
            return View(sorgu);
        }

        // POST: Kimlik/Edit/5
        [HttpPost] // Kullanıcı Ekleme-Silme-Güncelleme İşlemlerinden herhangi birini yapıp tamamlandığında karşılama bölümüne,veritabanı işlemleri ve klasöre kayıt işlemleri aşamasına geçilir.
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, kimlik kimlik, HttpPostedFileBase logoURL) // Kimlik Tablosundan kimlik adında değer türettik.Dosya işlemleri için HttpPostedFileBase kullandık.
        {
            if (ModelState.IsValid) // Model doğrulandıysa işlemlere geç
            {
                var k = dbModel.kimliks.Where(x => x.kimlikID == id).SingleOrDefault();

                int iFileSize = logoURL.ContentLength; // gelen Dosya boyutunu bir değişkene aktardık.

                var file = Path.GetFileName(logoURL.FileName); // dosya ismini alma işlemi 1
                //FileInfo file = new FileInfo(logoURL.FileName); // dosya ismini alma işlemi 2
                
                if (iFileSize < (1024 * 1024 * 5)) // MB Kontrolü
                {
                    var dosyayoluKontrol = System.IO.Path.Combine(Server.MapPath("~/Uploads/kimlik/")+file); // Mevcut Dizinde Aynı Dosya Kontrolü
                    if (!System.IO.File.Exists(dosyayoluKontrol)) // (! işareti var olan değerin tam tersini uygular.Yani Dosya belirtilen konumda yok ise); 
                    {
                        logoURL.SaveAs(dosyayoluKontrol); // Ana Dizine çık Ordan,Belirtilen klasöre resmi kaydet
                        k.logoURL = "/Uploads/kimlik/"+file; // Resmi veritabanına mevcut ismi ile kaydet

                        // Güncelleme yapılıcak kısımların eşleştirmelerini yaptık.
                    // Belirtilen ID'ye ait - Textbox'a girilen (Context Modelde,kimlik Sınıfında değişkende tutulan) içeriği veritabanında olan ile güncelle

                        k.title = kimlik.title; 
                        k.keywords = kimlik.keywords;
                        k.description = kimlik.description;
                        k.unvan = kimlik.unvan;

                        dbModel.SaveChanges();
                        TempData["uyari"] = "Güncelleme İşlemi başarılı bir şekilde gerçekleşti!";
                        return RedirectToAction("Index");
                    }
                    else // Aynı dosya var ise ;
                    {
                        TempData["uyari"] = "Yüklemeye çalıştığınız dosya sistemde kayıtlıdır.\n Lütfen Farklı bir dosya yükleyiniz..";
                    }
                }
                else
                {
                    TempData["uyari"] = "Dosya boyutunuz Maksimum 5 MB olmalıdır.";
                }
            }
            return View(kimlik);
        }

        // GET: Kimlik/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kimlik/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
