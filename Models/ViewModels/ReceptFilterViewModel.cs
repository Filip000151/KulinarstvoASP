using Microsoft.AspNetCore.Mvc.Rendering;

namespace KulinarstvoASP.Models.ViewModels
{
    public class ReceptFilterViewModel
    {
        public PagedResult<Recept> PagedRecept { get; set; }
        public SelectList KategorijaSelectList { get; set; }
        public int? SelectCategoryId { get; set; }
        public string SearchTerm {  get; set; }
        public string SortOrder { get; set; }
    }
}
