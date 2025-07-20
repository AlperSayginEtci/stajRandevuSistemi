using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
[Authorize(Roles = "Admin")]

public class DetayModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public DetayModel(ApplicationDbContext db) => _db = db;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Kullanici Kullanici { get; set; } = new();

    public string? HataMesaji { get; set; }

    public void OnGet()
    {
        Kullanici = _db.Kullanicilar.Find(Id)!;
    }

    public IActionResult OnPostSifreGuncelle(int KullaniciID, string AdminSifresi, string YeniSifre)
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(email))
        {
            HataMesaji = "Giriş bilgisi bulunamadı.";
            return Page();
        }

        var admin = _db.Kullanicilar.FirstOrDefault(x => x.Eposta == email);
        if (admin == null)
        {
            HataMesaji = "Admin kullanıcısı bulunamadı.";
            return Page();
        }

        // Şifre doğrulama
        var hasher = new PasswordHasher<Kullanici>();
        var result = hasher.VerifyHashedPassword(admin, admin.SifreHash, AdminSifresi);
        if (result != PasswordVerificationResult.Success)
        {
            HataMesaji = "Admin şifresi yanlış.";
            return Page();
        }

        //Şifre güncellemesi
        var kullanici = _db.Kullanicilar.Find(KullaniciID);
        if (kullanici != null)
        {
            kullanici.SifreHash = hasher.HashPassword(kullanici, YeniSifre);
            _db.SaveChanges();
        }

        return RedirectToPage(new { id = KullaniciID });
    }

    public IActionResult OnPostSil(int KullaniciID, string AdminSifresi)
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(email))
        {
            HataMesaji = "Giriş bilgisi bulunamadı.";
            return Page();
        }

        var admin = _db.Kullanicilar.FirstOrDefault(x => x.Eposta == email);
        if (admin == null)
        {
            HataMesaji = "Admin kullanıcı bulunamadı.";
            return Page();
        }

        // Şifre doğrulama
        var hasher = new PasswordHasher<Kullanici>();
        var result = hasher.VerifyHashedPassword(admin, admin.SifreHash, AdminSifresi);
        if (result != PasswordVerificationResult.Success)
        {
            HataMesaji = "Admin şifresi geçersiz.";
            return Page();
        }

        //İlişkili kayıtları temizle
        _db.Randevular_Kitap.RemoveRange(
            _db.Randevular_Kitap.Where(x => x.KullaniciID == KullaniciID));

        _db.Randevular_Oda.RemoveRange(
            _db.Randevular_Oda.Where(x => x.KullaniciID == KullaniciID));

        _db.OkunanKitaplar.RemoveRange(
            _db.OkunanKitaplar.Where(x => x.KullaniciID == KullaniciID));

        //Kullanıcıyı sil
        var kullanici = _db.Kullanicilar.Find(KullaniciID);
        if (kullanici != null)
        {
            _db.Kullanicilar.Remove(kullanici);
            _db.SaveChanges();
        }

        return RedirectToPage("Kullanicilar");
    }
}