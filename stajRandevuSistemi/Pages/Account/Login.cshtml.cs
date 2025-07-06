using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public LoginModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Eposta { get; set; }
            public string Password { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // 1) Kullanýcýyý bul
            var user = _db.Kullanicilar
                          .FirstOrDefault(u => u.Eposta == Input.Eposta);
            if (user == null)
            {
                ModelState.AddModelError("", "E-posta veya þifre yanlýþ.");
                return Page();
            }

            // 2) Þifreyi doðrula
            var hasher = new PasswordHasher<Kullanici>();
            var result = hasher.VerifyHashedPassword(user, user.SifreHash, Input.Password);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "E-posta veya þifre yanlýþ.");
                return Page();
            }

            // 3) Cookie oluþtur ve giriþ yaptýr
            var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.KullaniciID.ToString()),
        new Claim(ClaimTypes.Name, user.Eposta)
      };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme,
              new ClaimsPrincipal(identity)
            );

            return RedirectToPage("/Index"); // Giriþten sonra gideceðiniz sayfa
        }
    }
}