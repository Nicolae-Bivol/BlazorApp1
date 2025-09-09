using BlazorApp1.Application.Queries;
using BlazorApp1.Infrastructure.Data;
using BlazorApp1.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Handlers
{
     public class GetProductsQueryHandler
     {
          private readonly IDbContextFactory<ApplicationDbContext> _factory;
          public GetProductsQueryHandler(IDbContextFactory<ApplicationDbContext> factory) => _factory = factory;

          public async Task<List<ProductDto>> Handle(GetProductsQuery _)
          {
               using var ctx = _factory.CreateDbContext();
               return await ctx.Products.Select(p => new ProductDto
               {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock
               }).ToListAsync();
          }
     }
}
