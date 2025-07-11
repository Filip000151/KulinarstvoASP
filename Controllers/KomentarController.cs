using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KulinarstvoASP.Controllers
{
    public class KomentarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KomentarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int receptId, string sadrzaj)
        {
            if (string.IsNullOrEmpty(sadrzaj))
            {
                TempData["Error"] = "Komentar ne može biti prazan!";
                return RedirectToAction("Details", "Recept", new { id = receptId });
            }

            var userId = _userManager.GetUserId(User);

            var komentar = new Komentar
            {
                ReceptId = receptId,
                Sadrzaj = sadrzaj,
                UserId = userId,
                Datum = DateTime.Now
            };

            _context.Komentari.Add(komentar);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Recept", new {id = receptId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var komentar = await _context.Komentari.FindAsync(id);
            if (komentar != null) return NotFound();

            _context.Komentari.Remove(komentar);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Recept", new {id = komentar.ReceptId});
        }
    }
}
