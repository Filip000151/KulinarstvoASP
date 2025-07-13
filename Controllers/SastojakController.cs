using KulinarstvoASP.Constants;
using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var sastojci = await _context.Sastojci
                .Include(s => s.Recepti)
                .ToListAsync();
            return View(sastojci);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _context.Sastojci.FindAsync(id);

            try
            {
                _context.Sastojci.Remove(s);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest("Sastojak ne može biti obrisan jer se koristi u nekom receptu.");
            }
        }

        [Authorize(Roles = "Admin")]
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
