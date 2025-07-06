using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;

namespace stajRandevuSistemi.Pages.RandevuOda
{
    public class AlModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public AlModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int OdaID { get; set; }

        [BindProperty(SupportsGet = true)]
        public int KutuphaneID { get; set; }

        [BindProperty]
        public DateTime RandevuTarihi { get; set; }

        [BindProperty]
        public TimeSpan BaslangicSaati { get; set; }

        [BindProperty]
        public TimeSpan BitisSaati { get; set; }

        [BindProperty]
        public int Doluluk { get; set; }

        [BindProperty]
        public bool Paylasimli { get; set; }

        public Oda Oda { get; set; }
        public string KutuphaneAdi { get; set; }

        public void OnGet()
        {
            Oda = _db.Odalar.FirstOrDefault(o => o.OdaID == OdaID);
            var kutuphane = _db.Kutuphaneler.FirstOrDefault(k => k.KutuphaneID == KutuphaneID);
            KutuphaneAdi = kutuphane?.KutuphaneAdi ?? "Bilinmeyen";
        }

        public IActionResult OnPost()
        {       
            if (!ModelState.IsValid)
                return Page();
                if (BaslangicSaati < new TimeSpan(9, 0, 0) || BaslangicSaati > new TimeSpan(19, 0, 0))
    {
        ModelState.AddModelError("BaslangicSaati", "Baþlangýç saati 09:00 - 19:00 arasýnda olmalýdýr.");
    }

    if (BitisSaati < new TimeSpan(10, 0, 0) || BitisSaati > new TimeSpan(20, 0, 0))
    {
        ModelState.AddModelError("BitisSaati", "Bitiþ saati 10:00 - 20:00 arasýnda olmalýdýr.");
    }

    if (!ModelState.IsValid)
        return Page();


            var oda = _db.Odalar.FirstOrDefault(o => o.OdaID == OdaID);
            if (oda == null)
            {
                ModelState.AddModelError("", "Oda bulunamadý.");
                return Page();
            }

            int kullaniciID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var randevu = new stajRandevuSistemi.Models.RandevuOda
            {
                OdaID = OdaID,
                KullaniciID = kullaniciID,
                RandevuTarihi = RandevuTarihi,
                BaslangicSaati = BaslangicSaati,
                BitisSaati = BitisSaati,
                Doluluk = Paylasimli ? Doluluk : oda.Kapasite,
                Paylasimli = Paylasimli
            };

            _db.Randevular_Oda.Add(randevu);
            _db.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}