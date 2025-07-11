using System.ComponentModel.DataAnnotations;

namespace KulinarstvoASP.Models
{
    public class Recept
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv recepta je obavezan!")]
        [StringLength(100)]
        public string Naziv {  get; set; } = string.Empty;
        [Required(ErrorMessage = "Opis je obavezan!")]
        public string Opis { get; set; } = string.Empty;
        [Required(ErrorMessage = "Instrukcije su obavezne!")]
        public string Instrukcije { get; set; } = string.Empty;
        [Range(0, 300, ErrorMessage = "Trajanje spremanja mora biti između 0 i 300 minuta!")]
        public int? TrajanjeSpremanja { get; set; }
        [Range(0, 300, ErrorMessage = "Trajanje kuvanja mora biti između 0 i 300 minuta!")]
        public int? TrajanjeKuvanja { get; set; }

        [Required(ErrorMessage = "Slika je obavezna!")]
        [Url(ErrorMessage = "Unesite ispravan URL slike!")]
        public string Slika {  get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Kategorija je obavezna!")]
        public int? KategorijaId { get; set; }
        public Kategorija? Kategorija { get; set; }

        public List<ReceptSastojak> SastojciZaRecept { get; set; } = new();
        public List<Komentar> Komentari { get; set; } = new();
    }
}
