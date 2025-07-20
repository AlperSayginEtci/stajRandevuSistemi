using System.ComponentModel.DataAnnotations;

namespace stajRandevuSistemi.Models
{
    public class OkunanKitap
    {
        [Key]
        public int OkumaID { get; set; }

        public int KullaniciID { get; set; }
        public int KitapID { get; set; }
        public int KutuphaneID { get; set; }
        public DateTime AlinmaTarihi { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public bool GecTeslim { get; set; }

        public Kullanici Kullanici { get; set; }
        public Kitap Kitap { get; set; }
        public Kutuphane Kutuphane { get; set; }
    }
}
