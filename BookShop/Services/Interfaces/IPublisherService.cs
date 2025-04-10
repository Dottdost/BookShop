using BookShop.ADMIN.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<PublisherDto> CreatePublisherAsync(CreatePublisherDto dto);
        Task<PublisherDto> GetPublisherAsync(int id);
        Task<IEnumerable<PublisherDto>> GetPublishersAsync(int page = 1, int pageSize = 20);
        Task<PublisherDto> UpdatePublisherAsync(int id, UpdatePublisherDto dto);
        Task<bool> DeletePublisherAsync(int id);
    }
}