using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Application.Commands
{
     public class UpdateProductCommand
     {
          public int Id { get; set; }                 // produsul de modificat
          public string Name { get; set; } = string.Empty;
          public string Description { get; set; } = string.Empty;
          public decimal Price { get; set; }
          public int Stock { get; set; }
     }
}
