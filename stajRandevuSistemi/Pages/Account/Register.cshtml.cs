using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public RegisterModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Ad { get; set; }
            public string Soyad { get; set; }
            public string Eposta { get; set; }
            public string Password { get; set; }
        }

        public IActionResult OnGet() => Page();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            // E-posta kontrol
            if (_db.Kullanicilar.Any(u => u.Eposta == Input.Eposta))
            {
                ModelState.AddModelError("", "Bu e-posta zaten kayýtlý.");
                return Page();
            }

            // Þifreyi hashle
            var hasher = new PasswordHasher<Kullanici>();
            var user = new Kullanici
            {
                Ad = Input.Ad,
                Soyad = Input.Soyad,
                Eposta = Input.Eposta,
                SifreHash = hasher.HashPassword(null, Input.Password)
            };

            _db.Kullanicilar.Add(user);
            _db.SaveChanges();

            // Ýstersen otomatik giriþ de yapabilirsin. Þimdilik login sayfasýna yönlendir:
            return RedirectToPage("./Login");
        }
    }
}
