using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Admin.Kitaplar
{
    [Authorize(Roles = "Admin")]

    public class HavuzModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public HavuzModel(ApplicationDbContext db) => _db = db;

        public List<Kitap> Kitaplar { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Arama { get; set; }
        public Kitap? SonEklenenKitap { get; set; }
        public void OnGet()
        {
            var query = _db.Kitaplar.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Arama))
            {
                query = query.Where(k =>
                    k.KitapAdi.Contains(Arama) ||
                    k.Yazar.Contains(Arama) ||
                    k.Tur.Contains(Arama));
            }

            Kitaplar = query.ToList();
        }

        public async Task<IActionResult> OnPostEkleAsync(string KitapAdi, string Yazar, string Tur, IFormFile? KapakFoto)
        {
            if (string.IsNullOrWhiteSpace(KitapAdi)) return RedirectToPage();

            byte[]? kapakBytes = null;

            if (KapakFoto != null && KapakFoto.Length > 0)
            {
                using var ms = new MemoryStream();
                await KapakFoto.CopyToAsync(ms);
                kapakBytes = ms.ToArray();
            }

            var kitap = new Kitap
            {
                KitapAdi = KitapAdi,
                Yazar = Yazar,
                Tur = Tur,
                KapakFoto = kapakBytes
            };

            _db.Kitaplar.Add(kitap);
            await _db.SaveChangesAsync();
            SonEklenenKitap = kitap;

            return RedirectToPage();
        }

        public IActionResult OnPostSil(int KitapID)
        {
            
            var stoklar = _db.KutuphaneKitapStock.Where(s => s.KitapID == KitapID).ToList();
            _db.KutuphaneKitapStock.RemoveRange(stoklar);

            var kitap = _db.Kitaplar.Find(KitapID);
            if (kitap != null)
            {
                _db.Kitaplar.Remove(kitap);
            }

            _db.SaveChanges();
            return RedirectToPage();
        }
    }
}