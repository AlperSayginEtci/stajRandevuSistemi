using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
using System.Security.Claims;

namespace stajRandevuSistemi.Pages.Randevu
{
    [Authorize]
    public class RandevularimKitapModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public RandevularimKitapModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<RandevuKitapViewModel> Randevular { get; set; }

        public void OnGet()
        {
            int kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Randevular = (from r in _db.Randevular_Kitap
                          join k in _db.Kitaplar on r.KitapID equals k.KitapID
                          join kutuphane in _db.Kutuphaneler on r.KutuphaneID equals kutuphane.KutuphaneID
                          where r.KullaniciID == kullaniciId
                          orderby r.BashlangýcTarihi descending
                          select new RandevuKitapViewModel
                          {
                              RandevuID = r.KitapRandevuID,
                              KitapAdi = k.KitapAdi,
                              KutuphaneAdi = kutuphane.KutuphaneAdi,
                              RandevuTarihi = r.BashlangýcTarihi,
                              BitisTarihi = r.BitisTarihi
                          }).ToList();
        }

        public IActionResult OnPostTeslimEt(int RandevuID)
        {
            var randevu = _db.Randevular_Kitap.FirstOrDefault(r => r.KitapRandevuID == RandevuID);
            if (randevu == null)
                return NotFound();

            var teslimTarihi = DateTime.Now;
            bool gecTeslim = randevu.BitisTarihi.HasValue && teslimTarihi > randevu.BitisTarihi.Value;

            // Kitap stoðunu geri al
            var stok = _db.KutuphaneKitapStock.FirstOrDefault(s =>
                s.KitapID == randevu.KitapID && s.KutuphaneID == randevu.KutuphaneID);
            if (stok != null)
                stok.Stock += 1;

            // OkunanKitaplar db ekle
            var okuma = new OkunanKitap
            {
                KullaniciID = randevu.KullaniciID,
                KitapID = randevu.KitapID,
                KutuphaneID = randevu.KutuphaneID,
                AlinmaTarihi = randevu.BashlangýcTarihi,
                TeslimTarihi = teslimTarihi,
                GecTeslim = gecTeslim
            };
            _db.OkunanKitaplar.Add(okuma);

            _db.Randevular_Kitap.Remove(randevu);
            _db.SaveChanges();

            return RedirectToPage();
        }

        public class RandevuKitapViewModel
        {
            public int RandevuID { get; set; }
            public string KitapAdi { get; set; }
            public string KutuphaneAdi { get; set; }
            public DateTime RandevuTarihi { get; set; }
            public DateTime? BitisTarihi { get; set; }
        }
    }
}