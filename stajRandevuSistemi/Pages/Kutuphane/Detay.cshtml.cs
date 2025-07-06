using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Kutuphane
{
    public class DetayModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetayModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public string KutuphaneAdi { get; set; }

        public List<KitapGorunum> Kitaplar { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Arama { get; set; }


        public class KitapGorunum
        {
            public string KitapAdi { get; set; }
            public string Yazar { get; set; }
            public string Tur { get; set; }
            public int Stok { get; set; }
            public int KitapID { get; set; }
            public string KapakBase64 { get; set; }
        }

        public IActionResult OnGet(int id)
        {
            var kutuphane = _db.Kutuphaneler
                .Include(k => k.KitapStoklari)
                .ThenInclude(ks => ks.Kitap)
                .FirstOrDefault(k => k.KutuphaneID == id);

            if (kutuphane == null)
            {
                return RedirectToPage("/Index");
            }

            KutuphaneAdi = kutuphane.KutuphaneAdi;

            var kitaplar = kutuphane.KitapStoklari.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Arama))
            {
                var aramaLower = Arama.ToLower();
                kitaplar = kitaplar.Where(ks => ks.Kitap.KitapAdi.ToLower().Contains(aramaLower));
            }

            Kitaplar = kitaplar.Select(ks => new KitapGorunum
            {
                KitapAdi = ks.Kitap.KitapAdi,
                Yazar = ks.Kitap.Yazar,
                Tur = ks.Kitap.Tur,
                Stok = ks.Stock,
                KitapID = ks.Kitap.KitapID,
                KapakBase64 = ks.Kitap.KapakFoto != null
                    ? $"data:image/jpeg;base64,{Convert.ToBase64String(ks.Kitap.KapakFoto)}"
                    : null
            }).ToList();
            return Page();

        }
    }
}
