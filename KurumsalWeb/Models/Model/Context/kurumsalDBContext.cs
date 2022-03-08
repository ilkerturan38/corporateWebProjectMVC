using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using KurumsalWeb.Models.Model.Entity;

namespace KurumsalWeb.Models.Model.Context
{
    public class kurumsalDBContext:DbContext
    {
        public DbSet<admin> admins { get; set; }
        public DbSet<blog> blogs { get; set; }
        public DbSet<hakkimizda> hakkimizdas { get; set; }
        public DbSet<hizmet> hizmets { get; set; }
        public DbSet<iletisim> iletisims { get; set; }
        public DbSet<kategoriler> kategoriler { get; set; }
        public DbSet<kimlik> kimliks { get; set; }
        public DbSet<Slider> sliders { get; set; }
        public DbSet<yorum> yorums { get; set; }
 
        public System.Data.Entity.DbSet<KurumsalWeb.Models.Model.Entity.mail> mails { get; set; }
    }
}