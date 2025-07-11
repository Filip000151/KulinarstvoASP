using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KulinarstvoASP.Controllers
{
    public class SastojakController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SastojakController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sastojak model)
        {
            if (ModelState.IsValid)
            {
                var s = new Sastojak()
                {
                    Naziv = model.Naziv
                };

                _context.Sastojci.Add(s);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Index()
        {
            var sastojci = await _context.Sastojci.ToListAsync();
            return View(sastojci);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var s = await _context.Sastojci.FindAsync(id);

            if (s == null) return NotFound();

            var sastojak = new Sastojak
            {
                Id = s.Id,
                Naziv = s.Naziv
            };

            return View(sastojak);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sastojak model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var sastojak = await _context.Sastojci.FindAsync(id);
                if(sastojak == null) return NotFound();

                sastojak.Naziv = model.Naziv;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _context.Sastojci.FindAsync(id);

            _context.Sastojci.Remove(s);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var s = await _context.Sastojci.Include(s => s.Recepti).ThenInclude(rs => rs.Recept).FirstOrDefaultAsync(s => s.Id == id);

            if (s == null) return NotFound();

            var sastojak = new Sastojak()
            {
                Id = s.Id,
                Naziv = s.Naziv,
                Recepti = s.Recepti
            };

            return View(sastojak);
        }
    }
}
