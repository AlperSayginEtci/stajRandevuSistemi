using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
[Authorize(Roles = "Admin")]

public class KullanicilarModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public KullanicilarModel(ApplicationDbContext db) => _db = db;

    [BindProperty(SupportsGet = true)]
    public string? Arama { get; set; }

    public List<Kullanici> Kullanicilar { get; set; } = new();

    public void OnGet()
    {
        var query = _db.Kullanicilar.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Arama))
        {
            query = query.Where(k =>
                k.Ad.Contains(Arama!) ||
                k.Soyad.Contains(Arama!) ||
                k.Eposta.Contains(Arama!));
        }

        Kullanicilar = query.OrderBy(k => k.Ad).ThenBy(k => k.Soyad).ToList();
    }
}