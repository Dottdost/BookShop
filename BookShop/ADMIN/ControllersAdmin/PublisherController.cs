using BookShop.ADMIN.DTOs;
using BookShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ADMIN.ControllersAdmin
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        // Создание нового издателя
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePublisher([FromBody] CreatePublisherDto dto)
        {
            var publisher = await _publisherService.CreatePublisherAsync(dto);
            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.Id }, publisher);
        }

        // Получение издателя по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisher(int id)
        {
            var publisher = await _publisherService.GetPublisherAsync(id);

            if (publisher == null)
                return NotFound();

            return Ok(publisher);
        }

        // Получение всех издателей
        [HttpGet]
        public async Task<IActionResult> GetPublishers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var publishers = await _publisherService.GetPublishersAsync(page, pageSize);
            return Ok(publishers);
        }

        // Обновление данных издателя
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] UpdatePublisherDto dto)
        {
            var publisher = await _publisherService.UpdatePublisherAsync(id, dto);
            if (publisher == null)
                return NotFound();

            return Ok(publisher);
        }

        // Удаление издателя
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var result = await _publisherService.DeletePublisherAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
