namespace KulinarstvoASP.Models
{
    public class Recept
    {
        public int Id { get; set; }

        public string Naziv {  get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public string Instrukcije { get; set; } = string.Empty;

        public int? TrajanjeSpremanja { get; set; }
        public int? TrajanjeKuvanja { get; set; }

        public string Slika {  get; set; } = string.Empty;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } 

        public int? KategorijaId { get; set; }
        public Kategorija? Kategorija { get; set; }

        public List<ReceptSastojak> SastojciZaRecept { get; set; } = new();
        public List<Komentar> Komentari { get; set; } = new();
    }
}
