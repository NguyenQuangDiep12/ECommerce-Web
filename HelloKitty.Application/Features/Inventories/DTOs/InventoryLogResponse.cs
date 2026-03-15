using HelloKitty.Domain.Inventory.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Inventories.DTOs
{
    public class InventoryLogResponse
    {
        public Guid LogId { get; set; }
        public ChangeType ChangeType { get; set; }
        public int QuantityChange { get; set; }
        public int CurrentStock { get; set; }
        public Guid? ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
