using BlazorApp1.Application.Commands;
using BlazorApp1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Handlers
{
     public class UpdateProductCommandHandler
     {
          private readonly IDbContextFactory<ApplicationDbContext> _factory;
          public UpdateProductCommandHandler(IDbContextFactory<ApplicationDbContext> factory) => _factory = factory;

          public async Task<bool> Handle(UpdateProductCommand command)
          {
               using var ctx = _factory.CreateDbContext();
               var p = await ctx.Products.FirstOrDefaultAsync(x => x.Id == command.Id);
               if (p == null) return false;

               p.Name = command.Name;
               p.Description = command.Description;
               p.Price = command.Price;
               p.Stock = command.Stock;

               await ctx.SaveChangesAsync();
               return true;
          }
     }
}
