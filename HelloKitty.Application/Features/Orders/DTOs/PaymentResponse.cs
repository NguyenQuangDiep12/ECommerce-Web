using HelloKitty.Domain.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.DTOs
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
    }
}
