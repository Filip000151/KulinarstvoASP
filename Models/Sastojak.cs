using System.ComponentModel.DataAnnotations;

namespace KulinarstvoASP.Models
{
    public class Sastojak
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv sastojka je obavezan!")]
        [StringLength(100)]
        public string Naziv {  get; set; } = string.Empty;

        public List<ReceptSastojak> Recepti { get; set; } = new();
    }
}
