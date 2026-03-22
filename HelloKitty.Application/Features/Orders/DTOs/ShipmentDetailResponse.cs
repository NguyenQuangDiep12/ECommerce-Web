using HelloKitty.Domain.Orders.Enums;

namespace HelloKitty.Application.Features.Orders.DTOs
{
    public class ShipmentDetailResponse
    {
        public Guid ShipmentId { get; set; }
        public ShipmentProvider ShipmentProvider { get; set; }
        public string? TrackingCode { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
    }
}