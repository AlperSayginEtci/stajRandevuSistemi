using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
using System.Security.Claims;

namespace stajRandevuSistemi.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public ProfileModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public Kullanici Kullanici { get; set; }

        [BindProperty]
        public ChangePasswordModel Input { get; set; }

        public string Message { get; set; }

        public class ChangePasswordModel
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Kullanici = _db.Kullanicilar.FirstOrDefault(k => k.KullaniciID == id);
            return Page();
        }

        public IActionResult OnPost()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Kullanici = _db.Kullanicilar.FirstOrDefault(k => k.KullaniciID == id);

            if (Input.NewPassword != Input.ConfirmPassword)
            {
                ModelState.AddModelError("", "Yeni þifreler uyuþmuyor.");
                return Page();
            }

            var hasher = new PasswordHasher<Kullanici>();
            var result = hasher.VerifyHashedPassword(Kullanici, Kullanici.SifreHash, Input.CurrentPassword);

            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Mevcut þifre yanlýþ.");
                return Page();
            }

            Kullanici.SifreHash = hasher.HashPassword(Kullanici, Input.NewPassword);
            _db.SaveChanges();

            Message = "Þifreniz baþarýyla güncellendi.";
            return Page();
        }
    }
}
