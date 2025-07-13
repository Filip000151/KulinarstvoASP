using KulinarstvoASP.Constants;
using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KulinarstvoASP.Controllers
{
    public class KategorijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategorijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upravljaj()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kategorija model)
        {
            if (ModelState.IsValid)
            {
                var c = new Kategorija()
                {
                    Naziv = model.Naziv,
                    Opis = model.Opis,
                };

                _context.Kategorije.Add(c);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var kategorije = await _context.Kategorije.Include(k => k.Recepti).ToListAsync();
            return View(kategorije);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _context.Kategorije.FindAsync(id);

            if (c == null) return NotFound();

            var kategorija = new Kategorija
            {
                Id = c.Id,
                Naziv = c.Naziv,
                Opis = c.Opis
            };

            return View(kategorija);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kategorija model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var kategorija = await _context.Kategorije.FindAsync(id);

                if(kategorija == null) return NotFound();

                kategorija.Naziv = model.Naziv;
                kategorija.Opis = model.Opis;

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
            var c = await _context.Kategorije.FindAsync(id);

            try
            {
                _context.Kategorije.Remove(c);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest("Kategorija ne može biti obrisana jer se u njoj nalaze recepti.");
            }
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var c = await _context.Kategorije.Include(c => c.Recepti).FirstOrDefaultAsync(r => r.Id == id);

            if(c == null) return NotFound();

            var k = new Kategorija()
            {
                Id = c.Id,
                Naziv = c.Naziv,
                Opis = c.Opis,
                Recepti = c.Recepti
            };

            return View(k);
        }
    }
}
