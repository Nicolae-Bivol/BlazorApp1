using BlazorApp1.Application.Queries;
using BlazorApp1.Infrastructure.Data;
using BlazorApp1.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Handlers
{
     public class GetProductByIdQueryHandler
     {
          private readonly IDbContextFactory<ApplicationDbContext> _factory;
          public GetProductByIdQueryHandler(IDbContextFactory<ApplicationDbContext> factory) => _factory = factory;

          public async Task<ProductDto?> Handle(GetProductByIdQuery q)
          {
               using var ctx = _factory.CreateDbContext();
               return await ctx.Products
                   .Where(p => p.Id == q.Id)
                   .Select(p => new ProductDto
                   {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Stock = p.Stock
                   })
                   .FirstOrDefaultAsync();
          }
     }
}
