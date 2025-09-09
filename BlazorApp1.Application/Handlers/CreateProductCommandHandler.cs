using BlazorApp1.Application.Commands;
using BlazorApp1.Domain.Entities;
using BlazorApp1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Handlers
{
     public class CreateProductCommandHandler
     {
          private readonly IDbContextFactory<ApplicationDbContext> _factory;
          public CreateProductCommandHandler(IDbContextFactory<ApplicationDbContext> factory) => _factory = factory;

          public async Task<int> Handle(CreateProductCommand command)
          {
               using var ctx = _factory.CreateDbContext();
               var product = new Product
               {
                    Name = command.Name,
                    Description = command.Description,
                    Price = command.Price,
                    Stock = command.Stock
               };
               ctx.Products.Add(product);
               await ctx.SaveChangesAsync();
               return product.Id;
          }
     }
}
