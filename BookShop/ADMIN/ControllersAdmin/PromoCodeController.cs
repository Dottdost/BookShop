using BookShop.ADMIN.DTOs;
using BookShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        // Create a new promo code
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePromoCode([FromBody] CreatePromoCodeDTO dto)
        {
            try
            {
                var promoCode = await _promoCodeService.CreatePromoCodeAsync(dto);
                return Ok(promoCode);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get a promo code by code
        [HttpGet("Get/{code}")]
        public async Task<IActionResult> GetPromoCode(string code)
        {
            var promoCode = await _promoCodeService.GetPromoCodeAsync(code);

            if (promoCode == null)
                return NotFound(new { message = "Promo code not found" });

            return Ok(promoCode);
        }

        // Apply a promo code to an order (simplified)
        [HttpPost("Apply")]
        public async Task<IActionResult> ApplyPromoCode([FromBody] ApplyPromoCodeRequest request)
        {
            var isApplied = await _promoCodeService.ApplyPromoCodeAsync(request.Code, request.UserId);

            if (!isApplied)
                return BadRequest(new { message = "Invalid or expired promo code" });

            return Ok(new { message = "Promo code applied successfully" });
        }

        // Deactivate a promo code
        [HttpPost("Deactivate/{promoCodeId}")]
        public async Task<IActionResult> DeactivatePromoCode(int promoCodeId)
        {
            var result = await _promoCodeService.DeactivatePromoCodeAsync(promoCodeId);

            if (!result)
                return NotFound(new { message = "Promo code not found" });

            return Ok(new { message = "Promo code deactivated successfully" });
        }

        // Delete a promo code
        [HttpDelete("Delete/{promoCodeId}")]
        public async Task<IActionResult> DeletePromoCode(int promoCodeId)
        {
            var result = await _promoCodeService.DeletePromoCodeAsync(promoCodeId);

            if (!result)
                return NotFound(new { message = "Promo code not found" });

            return Ok(new { message = "Promo code deleted successfully" });
        }
    }

    public class ApplyPromoCodeRequest
    {
        public string Code { get; set; }
        public int UserId { get; set; }
    }
}
