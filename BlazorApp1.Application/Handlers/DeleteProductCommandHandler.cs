using BlazorApp1.Application.Commands;
using BlazorApp1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Application.Handlers
{
     public class DeleteProductCommandHandler
     {
          private readonly IDbContextFactory<ApplicationDbContext> _factory;
          public DeleteProductCommandHandler(IDbContextFactory<ApplicationDbContext> factory) => _factory = factory;

          public async Task<bool> Handle(DeleteProductCommand command)
          {
               using var ctx = _factory.CreateDbContext();
               var p = await ctx.Products.FirstOrDefaultAsync(x => x.Id == command.Id);
               if (p == null) return false;

               ctx.Products.Remove(p);
               await ctx.SaveChangesAsync();
               return true;
          }
     }
}
