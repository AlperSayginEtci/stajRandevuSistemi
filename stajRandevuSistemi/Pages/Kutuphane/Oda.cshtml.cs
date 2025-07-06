using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Kutuphane
{
    public class KutuphaneOdaModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public KutuphaneOdaModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int KutuphaneID { get; set; }

        public List<SelectListItem> KutuphaneListesi { get; set; }
        public List<Oda> Odalar { get; set; }
        public string KutuphaneAdi { get; set; }
        public void OnGet()
        {
            var kutuphane = _db.Kutuphaneler.FirstOrDefault(k => k.KutuphaneID == KutuphaneID);
            KutuphaneAdi = kutuphane?.KutuphaneAdi ?? "Bilinmeyen";
            KutuphaneListesi = _db.Kutuphaneler
                .Select(k => new SelectListItem
                {
                    Value = k.KutuphaneID.ToString(),
                    Text = k.KutuphaneAdi
                }).ToList();

            if (KutuphaneID > 0)
            {
                Odalar = _db.KutuphaneOdalar
                    .Include(ko => ko.Oda)
                    .Where(ko => ko.KutuphaneID == KutuphaneID)
                    .Select(ko => ko.Oda)
                    .ToList();
            }
        }
    }
}
