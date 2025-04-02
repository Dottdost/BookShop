using BookShop.ADMIN.DTOs;
using BookShop.ADMIN.ServicesAdmin.WarehouseServices;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ADMIN.ControllersAdmin;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _warehouseService.GetAllAsync();
        return Ok(items);
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> Update(Guid bookId, [FromBody] UpdateWarehouseStockDto dto)
    {
        var success = await _warehouseService.UpdateStockAsync(bookId, dto.Amount);
        if (!success) return NotFound();
        return Ok("Склад обновлён");
    }
}