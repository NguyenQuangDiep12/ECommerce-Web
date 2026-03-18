using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.DTOs
{
    public class CreateVariantRequest
    {
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<Guid> AttributeValueIds { get; set; }
    }
}
