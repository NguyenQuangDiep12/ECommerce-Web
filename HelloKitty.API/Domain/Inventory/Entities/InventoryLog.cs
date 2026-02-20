using HelloKitty.API.Domain.Catalog.Entities;
using HelloKitty.API.Domain.Inventory.Enums;
using System.Runtime.InteropServices.Marshalling;

namespace HelloKitty.API.Domain.Inventory.Entities
{
    public class InventoryLog
    {
        public Guid LogId { get; set; } = Guid.NewGuid();
        public Guid VariantId { get; set; }
        public ProductVariant productVariant { get; set; } = null!;
        public ChangeType changeType { get; set; }
        public int QuantityChange { get; set; }
        public int CurrentStock { get; set; }
        public Guid? ReferenceId { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
