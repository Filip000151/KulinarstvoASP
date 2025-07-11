using System.ComponentModel.DataAnnotations;

namespace KulinarstvoASP.Models
{
    public class ReceptSastojak
    {
        [Required]
        public int? ReceptId { get; set; }
        public Recept? Recept { get; set; }

        [Required]
        public int? SastojakId { get; set; }
        public Sastojak? Sastojak { get; set; }

        [Required(ErrorMessage = "Morate uneti količinu!")]
        public string Kolicina { get; set; } = string.Empty;
    }
}
