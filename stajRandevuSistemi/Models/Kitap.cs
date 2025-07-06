namespace stajRandevuSistemi.Models
{
    public class Kitap
    {
        public int KitapID { get; set; }
        public string? KitapAdi { get; set; }
        public string? Yazar { get; set; }
        public string? Tur { get; set; }
        public string? Yayinevi { get; set; }
        public string? Cevirmen { get; set; }
        public byte[] KapakFoto { get; set; }

        public ICollection<KutuphaneKitapStock> Kutuphaneler { get; set; }
        public ICollection<RandevuKitap> Randevular { get; set; }
    }
}
