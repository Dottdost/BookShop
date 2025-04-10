using BookShop.Data;
using BookShop.Data.Models;
using BookShop.Shared.DTO.Response;
using BookShop.ADMIN.DTOs;
using BookShop.Data.Contexts;
using BookShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Implementations
{
    public class PublisherService : IPublisherService
    {
        private readonly LibraryContext _context;

        public PublisherService(LibraryContext context)
        {
            _context = context;
        }

        // Создание нового издателя
        public async Task<PublisherDto> CreatePublisherAsync(CreatePublisherDto dto)
        {
            var publisher = new Publisher
            {
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber
            };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                PhoneNumber = publisher.PhoneNumber
            };
        }

        // Получение издателя по ID
        public async Task<PublisherDto> GetPublisherAsync(int id)
        {
            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publisher == null)
                return null;

            return new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                PhoneNumber = publisher.PhoneNumber
            };
        }

        // Получение всех издателей с пагинацией
        public async Task<IEnumerable<PublisherDto>> GetPublishersAsync(int page = 1, int pageSize = 20)
        {
            var publishers = await _context.Publishers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return publishers.Select(publisher => new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                PhoneNumber = publisher.PhoneNumber
            });
        }

        // Обновление данных издателя
        public async Task<PublisherDto> UpdatePublisherAsync(int id, UpdatePublisherDto dto)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return null;
            }

            publisher.Name = dto.Name;
            publisher.Address = dto.Address;
            publisher.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();

            return new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Address = publisher.Address,
                PhoneNumber = publisher.PhoneNumber
            };
        }

        // Удаление издателя
        public async Task<bool> DeletePublisherAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return false;
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
