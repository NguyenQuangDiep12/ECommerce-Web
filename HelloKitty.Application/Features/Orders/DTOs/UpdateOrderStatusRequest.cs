using HelloKitty.Domain.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.DTOs
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}
