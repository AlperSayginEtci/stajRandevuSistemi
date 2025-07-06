namespace stajRandevuSistemi.Models
{
    public class KutuphaneKitapStock
    {
        public int KitapID { get; set; }
        public Kitap Kitap { get; set; }

        public int KutuphaneID { get; set; }
        public Kutuphane Kutuphane { get; set; }

        public int Stock { get; set; }
    }
}
