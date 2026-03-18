using HelloKitty.Domain.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.DTOs
{
    public class CreateOrderRequest
    {
        public string ReceiverName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? VoucherCode { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
