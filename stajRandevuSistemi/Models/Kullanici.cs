namespace stajRandevuSistemi.Models
{
    public class Kullanici
    {
        public int KullaniciID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Eposta { get; set; }
        public string SifreHash { get; set; }
        public byte[]? ProfilFoto { get; set; }


        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }

}
