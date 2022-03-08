using KurumsalWeb.Models.Model.Context;
using KurumsalWeb.Models.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace KurumsalWeb.Controllers
{
    public class HakkimizdaController : Controller
    {
        kurumsalDBContext db = new kurumsalDBContext();
        // GET: Hakkimizda / Eğer Metotlar üzerinde hiçbir Attiribute olmassa veriler bize GET olarak gelir.(Sadece Verilerin Getirildiği Metot)
        public ActionResult Index()
        {
            return View(db.hakkimizdas.ToList());
        }
        public ActionResult Edit(int id)
        {
            var sorgu = db.hakkimizdas.Where(p => p.hakkimizdaID == id).FirstOrDefault();
            return View(sorgu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id,hakkimizda hakkimizda)
        {
            if (ModelState.IsValid) // Model doğrulandıysa işlemlere geç
            {
                try
                {
                    var sorgu = db.hakkimizdas.Where(p => p.hakkimizdaID == id).FirstOrDefault();
                    sorgu.aciklama = hakkimizda.aciklama;
                    db.SaveChanges();
                    TempData["uyari"] = "Güncelleme işlemi başarılı bir şekilde gerçekleşti.";
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    TempData["uyari"] = "Güncelleme işlemi sırasında hata ile karşılaşıldı."+ex;
                    
                }
                
            }
            return View(hakkimizda);
        }

    }
}