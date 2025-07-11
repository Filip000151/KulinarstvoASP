using System.ComponentModel.DataAnnotations;

namespace KulinarstvoASP.Models
{
    public class Kategorija
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv kategorije je obavezan!")]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;
        [Required(ErrorMessage = "Opis je obavezan!")]
        public string? Opis { get; set; } = string.Empty;

        public List<Recept> Recepti { get; set; } = new();
    }
}
