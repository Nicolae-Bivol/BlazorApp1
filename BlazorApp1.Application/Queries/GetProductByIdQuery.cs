using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Application.Queries
{
     // folosit pentru a lua un produs după Id
     public class GetProductByIdQuery
     {
          public int Id { get; set; }

          public GetProductByIdQuery(int id)
          {
               Id = id;
          }
     }
}
