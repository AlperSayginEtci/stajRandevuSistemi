using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;

namespace stajRandevuSistemi.Pages.Admin
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

        [BindProperty]
        public IFormFile YeniFoto { get; set; }

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

            if (Kullanici == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Kullanici = _db.Kullanicilar.FirstOrDefault(k => k.KullaniciID == id);
            if (Kullanici == null)
                return NotFound();

            // Þifre alanlarý doluysa kontrol et
            if (!string.IsNullOrWhiteSpace(Input?.CurrentPassword))
            {
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
                Message = "Þifreniz baþarýyla güncellendi.";
            }

            // Resim yükleme
            if (YeniFoto != null && YeniFoto.Length > 0)
            {
                using var ms = new MemoryStream();
                await YeniFoto.CopyToAsync(ms);
                Kullanici.ProfilFoto = ms.ToArray();
            }

            await _db.SaveChangesAsync();
            return Page();
        }


    }
}