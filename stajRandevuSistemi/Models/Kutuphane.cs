namespace stajRandevuSistemi.Models
{
    public class Kutuphane
    {
        public int KutuphaneID { get; set; }
        public string KutuphaneAdi { get; set; }
        public ICollection<KutuphaneOda> OdaBaglantilari { get; set; }
        public ICollection<KutuphaneKitapStock> KitapStoklari { get; set; }
        public ICollection<KutuphaneOda> Odalar { get; set; }
    }
}
