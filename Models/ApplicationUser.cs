using Microsoft.AspNetCore.Identity;

namespace KulinarstvoASP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Naziv {  get; set; }
    }
}
