using System.ComponentModel.DataAnnotations;

namespace KulinarstvoASP.Models
{
    public class Komentar
    {
        public int Id { get; set; }
        public string Sadrzaj { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public int? ReceptId { get; set; }
        public Recept? Recept { get; set; }

        [Required]
        public DateTime Datum { get; set; } = DateTime.UtcNow;
    }
}
