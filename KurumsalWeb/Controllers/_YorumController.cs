using KurumsalWeb.Models.Model.Context;
using KurumsalWeb.Models.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class _YorumController : Controller
    {
        kurumsalDBContext db = new kurumsalDBContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult onayliYorumlarPV()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var sorgu = db.yorums.Include("blog").Where(x => x.yorumOnay == true);
            return PartialView(sorgu.ToList());
        }

        public ActionResult onaysizYorumlarPV()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var sorgu = db.yorums.Include("blog").Where(x => x.yorumOnay == false);
            return PartialView(sorgu.ToList());
        }

        public ActionResult yorumAktif(int ID)
        {
            var sorgu = db.yorums.Find(ID);
            sorgu.yorumOnay = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult yorumPasif(int ID)
        {
            var sorgu = db.yorums.Find(ID);
            sorgu.yorumOnay = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult yorumGuncelleme(int ID)
        {
            var sorgu = db.yorums.Find(ID);
            return View(sorgu);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult yorumGuncelleme(int ID,yorum mevcutYorum)
        {
            try
            {
                var sorgu = db.yorums.Find(ID);
                sorgu.adsoyad = mevcutYorum.adsoyad;
                sorgu.email = mevcutYorum.email;
                sorgu.Icerik = mevcutYorum.Icerik;
                sorgu.yorumOnay = mevcutYorum.yorumOnay;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.hata = "Yorum güncelleme işlemi sırasında hata meydana geldi!" + ex;
            }
            return View();
        }

        [HttpGet]
        public ActionResult yorumSilme(int ID)
        {
            try
            {
                var sorgu = db.yorums.Find(ID);
                db.yorums.Remove(sorgu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.hata = "Yorum Silme işlemi sırasında hata meydana geldi!" + ex;
            }
            return View();
        }
    }
}