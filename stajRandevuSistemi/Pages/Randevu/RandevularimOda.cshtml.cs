using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using stajRandevuSistemi.Data;
using stajRandevuSistemi.Models;
using System.Security.Claims;

namespace stajRandevuSistemi.Pages.Randevu
{
    [Authorize]
    public class RandevularimOdaModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public RandevularimOdaModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<RandevuOdaViewModel> Randevular { get; set; }

        public void OnGet()
        {
            int kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Randevular = (from r in _db.Randevular_Oda
                          join o in _db.Odalar on r.OdaID equals o.OdaID
                          join ko in _db.KutuphaneOdalar on o.OdaID equals ko.OdaID
                          join k in _db.Kutuphaneler on ko.KutuphaneID equals k.KutuphaneID
                          where r.KullaniciID == kullaniciId
                          orderby r.RandevuTarihi descending, r.BaslangicSaati
                          select new RandevuOdaViewModel
                          {
                              OdaNo = o.OdaNo,
                              KutuphaneAdi = k.KutuphaneAdi,
                              RandevuTarihi = r.RandevuTarihi,
                              BaslangicSaati = r.BaslangicSaati,
                              BitisSaati = r.BitisSaati,
                              Paylasimli = r.Paylasimli,
                              Doluluk = r.Doluluk
                          }).ToList();
        }

        public class RandevuOdaViewModel
        {
            public string OdaNo { get; set; }
            public string KutuphaneAdi { get; set; }
            public DateTime RandevuTarihi { get; set; }
            public TimeSpan BaslangicSaati { get; set; }
            public TimeSpan BitisSaati { get; set; }
            public bool Paylasimli { get; set; }
            public int Doluluk { get; set; }
        }
    }
}