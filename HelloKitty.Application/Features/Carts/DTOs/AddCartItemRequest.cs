using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Carts.DTOs
{
    public class AddCartItemRequest
    {
        public Guid VarianId { get; set; }
        public int Quantity { get; set; }
    }
}
