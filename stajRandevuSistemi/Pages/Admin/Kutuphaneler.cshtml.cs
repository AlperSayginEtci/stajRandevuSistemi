using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Admin
{
    [Authorize(Roles = "Admin")]

    public class KutuphanelerModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public KutuphanelerModel(ApplicationDbContext db) => _db = db;

        public List<stajRandevuSistemi.Models.Kutuphane> KutuphaneList { get; set; } = new();

        [BindProperty]
        public string YeniKutuphaneAdi { get; set; }

        public void OnGet()
        {
            KutuphaneList = _db.Kutuphaneler.ToList();
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(YeniKutuphaneAdi))
            {
                _db.Kutuphaneler.Add(new stajRandevuSistemi.Models.Kutuphane { KutuphaneAdi = YeniKutuphaneAdi.Trim() });
                _db.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int SilID)
        {
            var kutuphane = _db.Kutuphaneler.Find(SilID);
            if (kutuphane != null)
            {
                // Ýliþkili odalarý sil
                var iliskiliOdalar = _db.KutuphaneOdalar.Where(ko => ko.KutuphaneID == SilID).ToList();
                _db.KutuphaneOdalar.RemoveRange(iliskiliOdalar);

                _db.Kutuphaneler.Remove(kutuphane);
                _db.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int EditID, string EditAd)
        {
            var kutuphane = _db.Kutuphaneler.Find(EditID);
            if (kutuphane != null)
            {
                kutuphane.KutuphaneAdi = EditAd;
                _db.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}