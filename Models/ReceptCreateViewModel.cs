using Microsoft.AspNetCore.Mvc.Rendering;

namespace KulinarstvoASP.Models
{
    public class ReceptCreateViewModel
    {
        public string Naziv {  get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public string Instrukcije { get; set; } = string.Empty;
        public int? TrajanjeSpremanja { get; set; }
        public int? TrajanjeKuvanja { get; set; }
        public string Slika { get; set; } = string.Empty;
        public int? KategorijaId {  get; set; }

        public List<SelectListItem> SviSastojci { get; set; } = new();
        public List<ReceptSastojakInput> Sastojci { get; set; } = new();
    }

    public class ReceptSastojakInput
    {
        public int? SastojakId { get; set; }
        public string Kolicina { get; set; } = string.Empty;
    }
}
