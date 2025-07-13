using KulinarstvoASP.Data;
using KulinarstvoASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KulinarstvoASP.Controllers
{
    [Route("api/Kategorija")]
    [ApiController]
    public class KategorijaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KategorijaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Kategorija>>> DohvatiSve()
        {
            var sve = await _context.Kategorije.ToListAsync();
            return Ok(sve);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Kategorija>> DohvatiJednu(int id)
        {
            var jednaKategorija = await _context.Kategorije.FirstOrDefaultAsync(k => k.Id == id);

            if (jednaKategorija == null) return NotFound();

            return Ok(jednaKategorija);
        }

        [HttpPost]
        public async Task<ActionResult<Kategorija>> KreirajNovu([FromBody] Kategorija input)
        {
            if (string.IsNullOrWhiteSpace(input.Naziv))
            {
                ModelState.AddModelError("Naziv", "Naziv kategorije je obavezan");

                return BadRequest(ModelState);
            }

            var nova = new Kategorija
            {
                Naziv = input.Naziv.Trim(),
                Opis = input.Opis.Trim()
            };

            _context.Kategorije.Add(nova);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(DohvatiJednu), new { id = nova.Id }, nova);
        }
        

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Obrisi(int id)
        {
            var k = await _context.Kategorije.FirstOrDefaultAsync(k => k.Id == id);

            if (k == null)
                return NotFound();

            try
            {
                _context.Kategorije.Remove(k);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Nije moguće obrisati kategoriju jer je povezana sa jednim ili više recepata.");
            }
        }
    }
}
