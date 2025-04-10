using BookShop.ADMIN.DTOs;
using BookShop.Data.Contexts;
using BookShop.Data.Models;
using BookShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Services.Implementations
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly LibraryContext _context;

        public PromoCodeService(LibraryContext context)
        {
            _context = context;
        }
        
        public async Task<PromoCodeResponseDTO> CreatePromoCodeAsync(CreatePromoCodeDTO dto)
        {
            var existingPromoCode = await _context.PromoCodes
                .FirstOrDefaultAsync(pc => pc.Code == dto.Code);
            
            if (existingPromoCode != null)
            {
                throw new Exception("Promo code already exists.");
            }

            // Создание нового промокода
            var promoCode = new PromoCode
            {
                Code = dto.Code,
                Discount = dto.Discount,
                ExpiryDate = dto.ExpiryDate,
                IsActive = true  
            };

            // Добавление промокода в базу данных
            _context.PromoCodes.Add(promoCode);
            await _context.SaveChangesAsync();

            return new PromoCodeResponseDTO
            {
                Id = promoCode.Id,
                Code = promoCode.Code,
                Discount = promoCode.Discount,
                ExpiryDate = promoCode.ExpiryDate,
                IsActive = true 
            };
        }


        public async Task<PromoCodeResponseDTO> GetPromoCodeAsync(string code)
        {
            var promoCode = await _context.PromoCodes
                .FirstOrDefaultAsync(pc => pc.Code == code);

            if (promoCode == null)
                return null; 

            return new PromoCodeResponseDTO
            {
                Id = promoCode.Id,
                Code = promoCode.Code,
                Discount = promoCode.Discount,
                ExpiryDate = promoCode.ExpiryDate,
                IsActive = promoCode.IsActive 
            };
        }

        // Применение промокода
        public async Task<bool> ApplyPromoCodeAsync(string code, int userId)
        {
            var promoCode = await _context.PromoCodes
                .FirstOrDefaultAsync(pc => pc.Code == code);

            // Проверка наличия промокода и его действительности
            if (promoCode == null || promoCode.ExpiryDate < DateTime.UtcNow || !promoCode.IsActive)
                return false;

            // Логика применения скидки в зависимости от бизнес-логики (например, в заказе)
            // В этой части можно будет добавить логику для скидки в заказ или применение к конкретному пользователю.

            
            return true;
        }
        
        public async Task<bool> DeletePromoCodeAsync(int promoCodeId)
        {
            var promoCode = await _context.PromoCodes.FindAsync(promoCodeId);

            if (promoCode == null)
                return false; 

            _context.PromoCodes.Remove(promoCode);  
            await _context.SaveChangesAsync();
            return true; 
        }
        
   
        public async Task<bool> DeactivatePromoCodeAsync(int promoCodeId)
        {
            var promoCode = await _context.PromoCodes.FindAsync(promoCodeId);

            if (promoCode == null)
                return false; 

            promoCode.IsActive = false;  
            await _context.SaveChangesAsync();
            return true;  
        }
    }
}
