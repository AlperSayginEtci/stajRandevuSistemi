namespace stajRandevuSistemi.Models
{
    public class Oda
    {
        public int OdaID { get; set; }
        public string OdaNo { get; set; }
        public int Kapasite { get; set; }
        public string Durum { get; set; }

        public ICollection<KutuphaneOda> KutuphaneBaglantilari { get; set; }
        public ICollection<RandevuOda> Randevular { get; set; }
    }
}
