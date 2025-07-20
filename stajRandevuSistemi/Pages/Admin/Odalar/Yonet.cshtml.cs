using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.Admin.Odalar
{
    [Authorize(Roles = "Admin")]

    public class YonetModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public YonetModel(ApplicationDbContext db) => _db = db;

        [BindProperty(SupportsGet = true)]
        public int KutuphaneID { get; set; }

        public string KutuphaneAdi { get; set; }
        public List<Oda> Odalar { get; set; } = new();

        public IActionResult OnGet()
        {
            var kutuphane = _db.Kutuphaneler.Find(KutuphaneID);
            if (kutuphane == null)
            {
                return NotFound(); // 404 
            }

            KutuphaneAdi = kutuphane.KutuphaneAdi;

            Odalar = _db.KutuphaneOdalar
                .Where(ko => ko.KutuphaneID == KutuphaneID && ko.Oda != null)
                .Select(ko => ko.Oda)
                .ToList();

            return Page();
        }

        public IActionResult OnPostEkle(int KutuphaneID, string OdaNo, int Kapasite, string Durum)
        {
            var yeniOda = new Oda { OdaNo = OdaNo, Kapasite = Kapasite, Durum = Durum };
            _db.Odalar.Add(yeniOda);
            _db.SaveChanges();

            _db.KutuphaneOdalar.Add(new KutuphaneOda
            {
                OdaID = yeniOda.OdaID,
                KutuphaneID = KutuphaneID
            });
            _db.SaveChanges();

            return RedirectToPage(new { kutuphaneId = KutuphaneID });
        }

        public IActionResult OnPostSil(int OdaID, int KutuphaneID)
        {
            var oda = _db.Odalar.Find(OdaID);
            if (oda != null)
            {
                var iliski = _db.KutuphaneOdalar.FirstOrDefault(x => x.OdaID == OdaID && x.KutuphaneID == KutuphaneID);
                if (iliski != null) _db.KutuphaneOdalar.Remove(iliski);

                _db.Odalar.Remove(oda);
                _db.SaveChanges();
            }

            return RedirectToPage(new { kutuphaneId = KutuphaneID });
        }
    }
}