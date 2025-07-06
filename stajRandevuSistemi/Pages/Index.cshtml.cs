using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace stajRandevuSistemi.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public string KullaniciAdi { get; set; }
        public List<stajRandevuSistemi.Models.Kutuphane> Kutuphaneler { get; set; }

        [BindProperty]
        public string RandevuTuru { get; set; }

        public string? HataMesaji { get; set; }


        public void OnGet()
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var kullanici = _db.Kullanicilar.FirstOrDefault(k => k.KullaniciID == kullaniciId);
            KullaniciAdi = kullanici?.Ad ?? "Kullanýcý";

            Kutuphaneler = _db.Kutuphaneler.ToList();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(RandevuTuru))
            {
                OnGet(); // Sayfa verilerini yeniden yükle
                HataMesaji = "Lütfen bir randevu türü seçin.";
                return Page();
            }

            return RandevuTuru switch
            {
                "kitap" => RedirectToPage("/Randevu/RandevularimKitap"),
                "oda" => RedirectToPage("/Randevu/RandevularimOda"),
                _ => Page()
            };
        }


    }
}