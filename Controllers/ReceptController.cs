using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using KulinarstvoASP.Models.ViewModels;
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
            if (ModelState.IsValid)
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

                foreach (var s in model.Sastojci)
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

            model.SviSastojci = _context.Sastojci
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Naziv
                }).ToList();

            ViewBag.Kategorije = new SelectList(_context.Kategorije, "Id", "Naziv", model.KategorijaId);
            return View(model);
        }

        public async Task<IActionResult> Index(int? categoryId, string searchTerm, string sortOrder, int page=1, int pageSize=9)
        {
            var receptQuery = _context.Recepti
                .Include(r => r.Kategorija)
                .Include(r => r.User)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                receptQuery = receptQuery.Where(r => r.KategorijaId ==  categoryId.Value);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                receptQuery = receptQuery.Where(r => r.Naziv.Contains(searchTerm));
            }
            receptQuery = sortOrder switch
            {
                "naziv_desc" => receptQuery.OrderByDescending(r => r.Naziv),
                "trajanje_spremanja" => receptQuery.OrderByDescending(r => r.TrajanjeSpremanja),
                "trajanje_kuvanja" => receptQuery.OrderByDescending(r => r.TrajanjeKuvanja),
                _ => receptQuery.OrderBy(r => r.Naziv)
            };

            var totalItems = await receptQuery.CountAsync();
            var recepti = await receptQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var kategorije = await _context.Kategorije.ToListAsync();

            var selectList = new SelectList(
                kategorije,
                "Id",
                "Naziv",
                categoryId);

            var pagedResult = new PagedResult<Recept>
            {
                Items = recepti,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            var viewModel = new ReceptFilterViewModel
            {
                PagedRecept = pagedResult,
                KategorijaSelectList = selectList,
                SearchTerm = searchTerm,
                SortOrder = sortOrder
            };

            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var recept = await _context.Recepti
                .Include(r => r.SastojciZaRecept)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recept == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (recept.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var r = new ReceptCreateViewModel
            {
                Naziv = recept.Naziv,
                Opis = recept.Opis,
                Instrukcije = recept.Instrukcije,
                TrajanjeSpremanja = recept.TrajanjeSpremanja,
                TrajanjeKuvanja = recept.TrajanjeKuvanja,
                Slika = recept.Slika,
                KategorijaId = recept.KategorijaId,

                Sastojci = recept.SastojciZaRecept.Select(rs => new ReceptSastojakInput
                {
                    SastojakId = rs.SastojakId,
                    Kolicina = rs.Kolicina
                }).ToList(),

                SviSastojci = _context.Sastojci
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Naziv
                    }).ToList()
            };

            ViewBag.Kategorije = new SelectList(_context.Kategorije, "Id", "Naziv", r.KategorijaId);
            return View(r);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReceptCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SviSastojci = _context.Sastojci
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Naziv
                    }).ToList();

                ViewBag.Kategorije = new SelectList(_context.Kategorije, "Id", "Naziv", model.KategorijaId);
                return View(model);
            }

            var recept = await _context.Recepti
                .Include(r => r.SastojciZaRecept)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(recept == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (recept.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            recept.Naziv = model.Naziv;
            recept.Opis = model.Opis;
            recept.Instrukcije = model.Instrukcije;
            recept.TrajanjeSpremanja = model.TrajanjeSpremanja;
            recept.TrajanjeKuvanja = model.TrajanjeKuvanja;
            recept.Slika = model.Slika;
            recept.KategorijaId = model.KategorijaId;

            _context.ReceptSastojci.RemoveRange(recept.SastojciZaRecept);

            foreach (var s in model.Sastojci)
            {
                if (s.SastojakId.HasValue && !string.IsNullOrWhiteSpace(s.Kolicina))
                {
                    _context.ReceptSastojci.Add(new ReceptSastojak
                    {
                        ReceptId = recept.Id,
                        SastojakId = s.SastojakId.Value,
                        Kolicina = s.Kolicina
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = recept.Id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _context.Recepti.FindAsync(id);

            var userId = _userManager.GetUserId(User);
            if (r.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();

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
