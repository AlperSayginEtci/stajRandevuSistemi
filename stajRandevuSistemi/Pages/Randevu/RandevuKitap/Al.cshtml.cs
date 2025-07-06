using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
using System.Security.Claims;

namespace stajRandevuSistemi.Pages.RandevuKitap
{
    [Authorize]
    public class AlModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public AlModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int KitapID { get; set; }

        [BindProperty(SupportsGet = true)]
        public int KutuphaneID { get; set; }

        [BindProperty]
        public DateTime? BitisTarihi { get; set; }

        public Kitap Kitap { get; set; }
        public string KutuphaneAdi { get; set; }

        public IActionResult OnGet()
        {
            if (!BitisTarihi.HasValue)
            {
                BitisTarihi = DateTime.Now.AddDays(1);
            }

            Kitap = _db.Kitaplar.FirstOrDefault(k => k.KitapID == KitapID);
            if (Kitap == null) return RedirectToPage("/Index");


            var kutuphane = _db.Kutuphaneler.FirstOrDefault(k => k.KutuphaneID == KutuphaneID);
            KutuphaneAdi = kutuphane?.KutuphaneAdi ?? "Bilinmeyen Kütüphane";

            return Page();
        }

        public IActionResult OnPost()
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var stokKaydi = _db.KutuphaneKitapStock
                .FirstOrDefault(s => s.KitapID == KitapID && s.KutuphaneID == KutuphaneID);

            if (stokKaydi == null || stokKaydi.Stock <= 0)
            {
                ModelState.AddModelError("", "Bu kitap þu anda stokta yok.");
                Kitap = _db.Kitaplar.FirstOrDefault(k => k.KitapID == KitapID);
                var kutuphane = _db.Kutuphaneler.FirstOrDefault(k => k.KutuphaneID == KutuphaneID);
                KutuphaneAdi = kutuphane?.KutuphaneAdi;
                return Page();
            }


            var randevu = new stajRandevuSistemi.Models.RandevuKitap

            {
                KitapID = KitapID,
                KutuphaneID = KutuphaneID,
                KullaniciID = kullaniciId,
                BashlangýcTarihi = DateTime.Now.Date,
                BitisTarihi = BitisTarihi
            };

            stokKaydi.Stock -= 1;

            _db.Randevular_Kitap.Add(randevu);
            _db.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}