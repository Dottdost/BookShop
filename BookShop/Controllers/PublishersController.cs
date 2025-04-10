using Microsoft.AspNetCore.Mvc;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using BookShop.Data.Models;
using AutoMapper;
using BookShop.ADMIN.DTOs;
using BookShop.Data.Contexts;

namespace BookShop.Controllers
{
    [Route("api/publishers")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public PublishersController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers()
        {
            var publishers = await _context.Publishers.ToListAsync();
            var publisherDtos = _mapper.Map<List<PublisherDto>>(publishers);
            return Ok(publisherDtos);
        }

        // GET: api/publishers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            var publisherDto = _mapper.Map<PublisherDto>(publisher);
            return Ok(publisherDto);
        }

        // POST: api/publishers
        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher(Publisher publisher)
        {
            // Перед добавлением проверим, существует ли такой издатель
            var existingPublisher = await _context.Publishers
                .FirstOrDefaultAsync(p => p.Name == publisher.Name);
            
            if (existingPublisher != null)
            {
                return BadRequest("Publisher with this name already exists.");
            }

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            // Возвращаем PublisherDto вместо Publisher
            var publisherDto = _mapper.Map<PublisherDto>(publisher);
            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.Id }, publisherDto);
        }

        // PUT: api/publishers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return BadRequest("Publisher ID mismatch.");
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Publishers.Any(e => e.Id == id))
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

        // DELETE: api/publishers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
