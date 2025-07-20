using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Admin.Kitaplar
{
    [Authorize(Roles = "Admin")]

    public class SecModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public SecModel(ApplicationDbContext db) => _db = db;

        public List<stajRandevuSistemi.Models.Kutuphane> Kutuphaneler { get; set; } = new();

        public void OnGet()
        {
            Kutuphaneler = _db.Kutuphaneler.ToList();
        }
    }
}