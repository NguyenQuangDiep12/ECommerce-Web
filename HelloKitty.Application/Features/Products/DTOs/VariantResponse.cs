using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.DTOs
{
    public class VariantResponse
    {
        public Guid VariantId { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public List<VariantAttributeResponse> Attributes = new List<VariantAttributeResponse>();
    }
}
