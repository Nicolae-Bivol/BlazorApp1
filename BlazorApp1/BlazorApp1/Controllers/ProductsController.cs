using BlazorApp1.Infrastructure.Data;
using BlazorApp1.Shared.DTOs;
using BlazorApp1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Controllers;

[ApiController]
[Route("api/[controller]")]
[IgnoreAntiforgeryToken]
public class ProductsController : ControllerBase
{
     private readonly ApplicationDbContext _db;
     public ProductsController(ApplicationDbContext db) => _db = db;

     [HttpGet]
     public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
         => await _db.Products
             .Select(p => new ProductDto
             {
                  Id = p.Id,
                  Name = p.Name,
                  Description = p.Description,
                  Price = p.Price,
                  Stock = p.Stock
             })
             .ToListAsync();

     [HttpGet("{id:int}")]
     public async Task<ActionResult<ProductDto>> GetById(int id)
     {
          var p = await _db.Products.FindAsync(id);
          if (p is null) return NotFound();

          return new ProductDto
          {
               Id = p.Id,
               Name = p.Name,
               Description = p.Description,
               Price = p.Price,
               Stock = p.Stock
          };
     }

     [HttpPost]
     public async Task<ActionResult<ProductDto>> Post(ProductDto dto)
     {
          var p = new Product
          {
               Name = dto.Name,
               Description = dto.Description,
               Price = dto.Price,
               Stock = dto.Stock
          };
          _db.Products.Add(p);
          await _db.SaveChangesAsync();

          dto.Id = p.Id;
          return CreatedAtAction(nameof(GetById), new { id = p.Id }, dto);
     }

     [HttpPut("{id:int}")]
     public async Task<IActionResult> Put(int id, ProductDto dto)
     {
          var p = await _db.Products.FindAsync(id);
          if (p is null) return NotFound();

          p.Name = dto.Name;
          p.Description = dto.Description;
          p.Price = dto.Price;
          p.Stock = dto.Stock;

          await _db.SaveChangesAsync();
          return NoContent();
     }

     [HttpDelete("{id:int}")]
     public async Task<IActionResult> Delete(int id)
     {
          var p = await _db.Products.FindAsync(id);
          if (p is null) return NotFound();

          _db.Products.Remove(p);
          await _db.SaveChangesAsync();
          return NoContent();
     }
}
