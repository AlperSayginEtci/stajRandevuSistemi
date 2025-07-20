using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Admin.Kitaplar
{
    [Authorize(Roles = "Admin")]

    public class YonetModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public YonetModel(ApplicationDbContext db) => _db = db;

        [BindProperty(SupportsGet = true)]
        public int KutuphaneID { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Arama { get; set; }

        public string KutuphaneAdi { get; set; } = "";
        public List<KitapViewModel> KutuphaneKitaplari { get; set; } = new();
        public List<Kitap> TumKitaplar { get; set; } = new();

        public void OnGet()
        {
            var kutuphane = _db.Kutuphaneler.Find(KutuphaneID);
            if (kutuphane == null)
            {
                RedirectToPage("/Admin/Kitaplar/Sec");
                return;
            }

            KutuphaneAdi = kutuphane.KutuphaneAdi;
            TumKitaplar = _db.Kitaplar.ToList();

            var kitaplar = from kk in _db.KutuphaneKitapStock
                           join k in _db.Kitaplar on kk.KitapID equals k.KitapID
                           where kk.KutuphaneID == KutuphaneID
                           select new KitapViewModel
                           {
                               KitapID = k.KitapID,
                               KitapAdi = k.KitapAdi,
                               Yazar = k.Yazar,
                               Tur = k.Tur,
                               Stock = kk.Stock,
                               KapakFoto = k.KapakFoto

                           };

            if (!string.IsNullOrWhiteSpace(Arama))
            {
                kitaplar = kitaplar.Where(k =>
                    k.KitapAdi.Contains(Arama) ||
                    k.Yazar.Contains(Arama) ||
                    k.Tur.Contains(Arama));
            }

            KutuphaneKitaplari = kitaplar.ToList();
        }

        public IActionResult OnPostEkle(int KutuphaneID, int KitapID, int Stock)
        {
            var mevcut = _db.KutuphaneKitapStock
                .FirstOrDefault(x => x.KutuphaneID == KutuphaneID && x.KitapID == KitapID);

            if (mevcut != null)
            {
                mevcut.Stock += Stock;
            }
            else
            {
                _db.KutuphaneKitapStock.Add(new KutuphaneKitapStock
                {
                    KutuphaneID = KutuphaneID,
                    KitapID = KitapID,
                    Stock = Stock
                });
            }

            _db.SaveChanges();
            return RedirectToPage(new { kutuphaneId = KutuphaneID });
        }

        public IActionResult OnPostSil(int KutuphaneID, int KitapID)
        {
            var iliski = _db.KutuphaneKitapStock
                .FirstOrDefault(x => x.KutuphaneID == KutuphaneID && x.KitapID == KitapID);

            if (iliski != null)
            {
                _db.KutuphaneKitapStock.Remove(iliski);
                _db.SaveChanges();
            }

            return RedirectToPage(new { kutuphaneId = KutuphaneID });
        }
        public IActionResult OnPostGuncelleStok(int KutuphaneID, int KitapID, int YeniStok)
        {
            var stok = _db.KutuphaneKitapStock.FirstOrDefault(s => s.KutuphaneID == KutuphaneID && s.KitapID == KitapID);
            if (stok != null)
            {
                stok.Stock = YeniStok;
                _db.SaveChanges();
            }

            return RedirectToPage(new { kutuphaneId = KutuphaneID });
        }

        public class KitapViewModel
        {
            public int KitapID { get; set; }
            public string KitapAdi { get; set; }
            public string Yazar { get; set; }
            public string Tur { get; set; }
            public int Stock { get; set; }
            public byte[]? KapakFoto { get; set; }
        }
    }
}