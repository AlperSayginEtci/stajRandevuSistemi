using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using stajRandevuSistemi.Data;
using System.Security.Claims;

namespace stajRandevuSistemi.Pages.Account
{
    [Authorize]
    public class OkunanKitaplarModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public OkunanKitaplarModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<OkunanKitapViewModel> Kitaplar { get; set; } = new();

        public void OnGet()
        {
            int kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var kitaplar = _db.OkunanKitaplar
              .Where(o => o.KullaniciID == 3)
                 .Include(o => o.Kitap)
                 .ToList() // 🔑 burada sorgu veritabanından çekilir

                 .GroupBy(o => o.KitapID)
                 .Select(g => g.OrderByDescending(x => x.TeslimTarihi).First())
                 .Select(o => new OkunanKitapViewModel
                 {
                      KitapAdi = o.Kitap.KitapAdi,
                      Yazar = o.Kitap.Yazar,
                      KapakFoto = o.Kitap.KapakFoto,
                      AlinmaTarihi = o.AlinmaTarihi,
                      TeslimTarihi = o.TeslimTarihi
                 })
                  .ToList();
            Kitaplar = kitaplar;
        }

        public class OkunanKitapViewModel
        {
            public string KitapAdi { get; set; }
            public string Yazar { get; set; }
            public byte[] KapakFoto { get; set; }
            public DateTime AlinmaTarihi { get; set; }
            public DateTime TeslimTarihi { get; set; }
        }
    }
}