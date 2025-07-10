namespace KulinarstvoASP.Models
{
    public class Kategorija
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string? Opis { get; set; } = string.Empty;

        public List<Recept> Recepti { get; set; } = new();
    }
}
