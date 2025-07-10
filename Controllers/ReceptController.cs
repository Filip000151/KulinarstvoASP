using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KulinarstvoASP.Controllers
{
    public class ReceptController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceptController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            var kategorije = _context.Kategorije.ToList();
            ViewBag.Kategorije = new SelectList(kategorije, "Id", "Naziv");
            return View();
        }

        


    }
}
