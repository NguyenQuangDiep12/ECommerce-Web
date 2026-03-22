using HelloKitty.Domain.Orders.Enums;

namespace HelloKitty.Application.Features.Orders.DTOs
{
    public class CreateShipmentRequest
    {
        public ShipmentProvider ShipmentProvider { get; set; }
        public string? TrackingCode { get; set; }
    }
}