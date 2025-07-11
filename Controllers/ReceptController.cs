using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KulinarstvoASP.Controllers
{
    public class ReceptController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReceptController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = new ReceptCreateViewModel
            {
                SviSastojci = _context.Sastojci
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Naziv
                    }).ToList(),

                Sastojci = new List<ReceptSastojakInput>
                {
                    new()
                }
            };
            var kategorije = _context.Kategorije.ToList();
            ViewBag.Kategorije = new SelectList(kategorije, "Id", "Naziv");
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceptCreateViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            var r = new Recept
            {
                Naziv = model.Naziv,
                Opis = model.Opis,
                Instrukcije = model.Instrukcije,
                TrajanjeSpremanja = model.TrajanjeSpremanja,
                TrajanjeKuvanja = model.TrajanjeKuvanja,
                Slika = model.Slika,
                UserId = userId,
                KategorijaId = model.KategorijaId
            };

            _context.Recepti.Add(r);
            await _context.SaveChangesAsync();

            foreach(var s in model.Sastojci)
            {
                if (s.SastojakId > 0 && !string.IsNullOrWhiteSpace(s.Kolicina))
                {
                    _context.ReceptSastojci.Add(new ReceptSastojak
                    {
                        ReceptId = r.Id,
                        SastojakId = s.SastojakId,
                        Kolicina = s.Kolicina
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Index()
        {
            var recepti = await _context.Recepti.ToListAsync();
            return View(recepti);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var recept = await _context.Recepti.FindAsync(id);
            if (recept == null) return NotFound();

            var r = new Recept
            {
                Naziv = recept.Naziv,
                Opis = recept.Opis,
                Instrukcije = recept.Instrukcije,
                TrajanjeSpremanja = recept.TrajanjeSpremanja,
                TrajanjeKuvanja = recept.TrajanjeKuvanja,
                Slika = recept.Slika,
                UserId = recept.UserId,
                KategorijaId = recept.KategorijaId
            };

            ViewBag.Kategorije = new SelectList(_context.Kategorije, "Id", "Naziv", r.KategorijaId);
            return View(r);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recept model)
        {
            if (id != model.Id) return NotFound();

            var recept = await _context.Recepti.FindAsync(id);

            if(recept == null) return NotFound();

            recept.Naziv = model.Naziv;
            recept.Opis = model.Opis;
            recept.Instrukcije = model.Instrukcije;
            recept.TrajanjeSpremanja = model.TrajanjeSpremanja;
            recept.TrajanjeKuvanja = model.TrajanjeKuvanja;
            recept.Slika = model.Slika;
            recept.KategorijaId = model.KategorijaId;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _context.Recepti.FindAsync(id);

            _context.Recepti.Remove(r);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var r = await _context.Recepti
                .Include(r => r.Kategorija)
                .Include(r => r.User)
                .Include(r => r.SastojciZaRecept)
                    .ThenInclude(rs => rs.Sastojak)
                .Include(r => r.Komentari)
                    .ThenInclude(k => k.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (r == null) return NotFound();

            var recept = new Recept
            {
                Id = r.Id,
                Naziv = r.Naziv,
                Opis = r.Opis,
                Instrukcije = r.Instrukcije,
                TrajanjeSpremanja = r.TrajanjeSpremanja,
                TrajanjeKuvanja = r.TrajanjeKuvanja,
                Slika = r.Slika,
                UserId = r.UserId,
                User = r.User,
                KategorijaId = r.KategorijaId,
                Kategorija = r.Kategorija,
                SastojciZaRecept = r.SastojciZaRecept,
                Komentari = r.Komentari
            };

            return View(recept);
        }
    }
}
