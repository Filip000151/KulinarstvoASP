using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

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

        public async Task<IActionResult> Index()
        {
            var kategorije = await _context.Kategorije.ToListAsync();
            return View(kategorije);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Kategorije.FindAsync(id);

            _context.Kategorije.Remove(c);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

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
