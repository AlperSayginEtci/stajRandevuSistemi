using stajRandevuSistemi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace stajRandevuSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kutuphane> Kutuphaneler { get; set; }
        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<KutuphaneKitapStock> KutuphaneKitapStock { get; set; }
        public DbSet<Oda> Odalar { get; set; }
        public DbSet<KutuphaneOda> KutuphaneOdalar { get; set; }
        public DbSet<RandevuKitap> Randevular_Kitap { get; set; }
        public DbSet<RandevuOda> Randevular_Oda { get; set; }
        public DbSet<OkunanKitap> OkunanKitaplar { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<RandevuKitap>()
                .HasKey(r => r.KitapRandevuID);

            modelBuilder.Entity<KutuphaneKitapStock>()
                .HasKey(k => new { k.KitapID, k.KutuphaneID });

            modelBuilder.Entity<KutuphaneOda>()
    .HasKey(ko => new { ko.KutuphaneID, ko.OdaID });

            modelBuilder.Entity<KutuphaneOda>()
                .HasOne(ko => ko.Kutuphane)
                .WithMany(k => k.OdaBaglantilari)
                .HasForeignKey(ko => ko.KutuphaneID);

            modelBuilder.Entity<KutuphaneOda>()
                .HasOne(ko => ko.Oda)
                .WithMany(o => o.KutuphaneBaglantilari)
                .HasForeignKey(ko => ko.OdaID);



            modelBuilder.Entity<RandevuOda>()
                 .HasKey(r => r.OdaRandevuID);




            base.OnModelCreating(modelBuilder);
        }


    }



}
