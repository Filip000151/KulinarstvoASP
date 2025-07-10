namespace KulinarstvoASP.Models
{
    public class Sastojak
    {
        public int Id { get; set; }
        public string Naziv {  get; set; } = string.Empty;

        public List<ReceptSastojak> Recepti { get; set; } = new();
    }
}
