using Microsoft.AspNetCore.Mvc;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using BookShop.Data.Models;

namespace BookShop.Controllers
{
    [Route("api/promocodes")]
    [ApiController]
    public class PromoCodesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public PromoCodesController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoCode>>> GetPromoCodes()
        {
            return await _context.PromoCodes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromoCode>> GetPromoCode(Guid id)
        {
            var promoCode = await _context.PromoCodes.FindAsync(id);
            if (promoCode == null)
            {
                return NotFound();
            }
            return promoCode;
        }

        [HttpPost]
        public async Task<ActionResult<PromoCode>> CreatePromoCode(PromoCode promoCode)
        {
            _context.PromoCodes.Add(promoCode);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPromoCode), new { id = promoCode.Id }, promoCode);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromoCode(Guid id, PromoCode promoCode)
        {
            if (id != promoCode.Id)
            {
                return BadRequest();
            }

            _context.Entry(promoCode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PromoCodes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromoCode(Guid id)
        {
            var promoCode = await _context.PromoCodes.FindAsync(id);
            if (promoCode == null)
            {
                return NotFound();
            }

            _context.PromoCodes.Remove(promoCode);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
