using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Carts.DTOs
{
    public class CartItemResponse
    {
        public Guid CartItemId { get; set; }
        public Guid VariantId { get; set; }
        public string ProductName { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
    }
}
