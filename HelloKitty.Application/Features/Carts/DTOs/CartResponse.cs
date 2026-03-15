using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Carts.DTOs
{
    public class CartResponse
    {
        public Guid CartId { get; set; }
        public List<CartItemResponse> items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
