using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.DTOs
{
    public class OrderItemResponse
    {
        public Guid OrderItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public uint Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
