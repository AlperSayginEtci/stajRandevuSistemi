namespace stajRandevuSistemi.Models
{
    public class RandevuKitap
    {
        public int KitapRandevuID { get; set; }

        public int KitapID { get; set; }
        public Kitap Kitap { get; set; }

        public int KutuphaneID { get; set; }
        public Kutuphane Kutuphane { get; set; }

        public int KullaniciID { get; set; }
        public Kullanici Kullanici { get; set; }

        public DateTime BashlangıcTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public DateTime? TeslimTarihi { get; set; }
    }
}
