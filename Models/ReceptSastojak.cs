namespace KulinarstvoASP.Models
{
    public class ReceptSastojak
    {
        public int? ReceptId { get; set; }
        public Recept? Recept { get; set; }

        public int? SastojakId { get; set; }
        public Sastojak? Sastojak { get; set; }

        public string Kolicina { get; set; } = string.Empty;
    }
}
