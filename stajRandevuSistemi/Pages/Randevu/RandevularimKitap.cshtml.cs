using Microsoft.AspNetCore.Authorization;
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
                              KitapAdi = k.KitapAdi,
                              KutuphaneAdi = kutuphane.KutuphaneAdi,
                              RandevuTarihi = r.BashlangýcTarihi,
                              BitisTarihi = r.BitisTarihi
                          }).ToList();
        }

        public class RandevuKitapViewModel
        {
            public string KitapAdi { get; set; }
            public string KutuphaneAdi { get; set; }
            public DateTime RandevuTarihi { get; set; }
            public DateTime? BitisTarihi { get; set; }
        }
    }
}