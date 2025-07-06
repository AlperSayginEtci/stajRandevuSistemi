using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
using System.Security.Claims;

namespace stajRandevuSistemi.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public ChangePasswordModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public string Message { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Input.NewPassword != Input.ConfirmPassword)
            {
                ModelState.AddModelError("", "Yeni þifreler uyuþmuyor.");
                return Page();
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _db.Kullanicilar.FirstOrDefault(k => k.KullaniciID == userId);

            var hasher = new PasswordHasher<Kullanici>();
            var result = hasher.VerifyHashedPassword(user, user.SifreHash, Input.CurrentPassword);

            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Mevcut þifre yanlýþ.");
                return Page();
            }

            user.SifreHash = hasher.HashPassword(user, Input.NewPassword);
            _db.SaveChanges();

            Message = "Þifreniz baþarýyla güncellendi.";
            return Page();
        }
    }
}