namespace stajRandevuSistemi.Models
{
    public class KutuphaneOda
    {
        public int OdaID { get; set; }
        public Oda Oda { get; set; }

        public int KutuphaneID { get; set; }
        public Kutuphane Kutuphane { get; set; }

    }
}
