namespace KulinarstvoASP.Models
{
    public class Komentar
    {
        public int Id { get; set; }
        public string Sadrzaj { get; set; } = string.Empty;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? ReceptId { get; set; }
        public Recept? Recept { get; set; }

        public DateTime Datum { get; set; } = DateTime.UtcNow;
    }
}
