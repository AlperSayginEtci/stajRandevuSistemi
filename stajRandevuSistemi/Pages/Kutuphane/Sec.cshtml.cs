using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using stajRandevuSistemi.Data;

namespace stajRandevuSistemi.Pages.Kutuphane
{
    public class KutuphaneSecModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public KutuphaneSecModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int KutuphaneID { get; set; }

        [BindProperty]
        public string RandevuTuru { get; set; }

        public string KutuphaneAdi { get; set; }

        public void OnGet(int id)
        {
            KutuphaneID = id; 

            var kutuphane = _db.Kutuphaneler.FirstOrDefault(k => k.KutuphaneID == id);
            KutuphaneAdi = kutuphane?.KutuphaneAdi ?? "Bilinmeyen";
        }

        public IActionResult OnPost()
        {
            if (RandevuTuru == "kitap")
                return RedirectToPage("/Kutuphane/Detay", new { id = KutuphaneID });

            if (RandevuTuru == "oda")
                return RedirectToPage("/Kutuphane/Oda", new {  KutuphaneID });

            return Page();
        }
    }
}
