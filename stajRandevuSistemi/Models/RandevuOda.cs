namespace stajRandevuSistemi.Models
{
    public class RandevuOda
    {
        public int OdaRandevuID { get; set; }

        public int OdaID { get; set; }
        public Oda Oda { get; set; }

        public int KullaniciID { get; set; }
        public Kullanici Kullanici { get; set; }

        public DateTime RandevuTarihi { get; set; }
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }

        public int Doluluk { get; set; }
        public bool Paylasimli { get; set; }
    }
}
