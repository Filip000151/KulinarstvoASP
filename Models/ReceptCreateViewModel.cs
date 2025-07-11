using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KulinarstvoASP.Models
{
    public class ReceptCreateViewModel
    {
        [Required(ErrorMessage = "Naziv recepta je obavezan!")]
        [StringLength(100)]
        public string Naziv {  get; set; } = string.Empty;
        [Required(ErrorMessage = "Opis recepta je obavezan!")]
        public string Opis { get; set; } = string.Empty;
        [Required(ErrorMessage = "Instrukcije su obavezne!")]
        public string Instrukcije { get; set; } = string.Empty;
        [Range(0, 300, ErrorMessage = "Trajanje spremanja mora biti između 0 i 300 minuta!")]
        public int? TrajanjeSpremanja { get; set; }
        [Range(0, 300, ErrorMessage = "Trajanje kuvanja mora biti između 0 i 300 minuta!")]
        public int? TrajanjeKuvanja { get; set; }
        [Required(ErrorMessage = "Slika je obavezna!")]
        [Url(ErrorMessage = "Unesite ispravan URL slike!")]
        public string Slika { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kategorija je obavezna!")]
        public int? KategorijaId {  get; set; }

        public List<SelectListItem> SviSastojci { get; set; } = new();
        public List<ReceptSastojakInput> Sastojci { get; set; } = new();
    }

    public class ReceptSastojakInput
    {
        public int? SastojakId { get; set; }
        public string? Kolicina { get; set; } = string.Empty;
    }
}
