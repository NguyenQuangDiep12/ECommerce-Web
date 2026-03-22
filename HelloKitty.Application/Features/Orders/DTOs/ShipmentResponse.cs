using HelloKitty.Domain.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.DTOs
{
    public class ShipmentResponse
    {
        public Guid ShipmentId { get; set; }
        public ShipmentStatus Status { get; set; }
        public string? TrackingCode { get; set; }
    }
}
