using BookShop.ADMIN.DTOs;

namespace BookShop.Services.Interfaces
{
    public interface IPromoCodeService
    {
        Task<PromoCodeResponseDTO> CreatePromoCodeAsync(CreatePromoCodeDTO dto);
        Task<PromoCodeResponseDTO> GetPromoCodeAsync(string code);
        Task<bool> ApplyPromoCodeAsync(string code, int userId);
        Task<bool> DeletePromoCodeAsync(int promoCodeId);
        
        Task<bool> DeactivatePromoCodeAsync(int promoCodeId);  
    }
}