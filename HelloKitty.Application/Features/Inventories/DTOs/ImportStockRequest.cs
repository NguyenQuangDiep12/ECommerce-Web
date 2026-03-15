using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Inventories.DTOs
{
    public class ImportStockRequest
    {
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
    }
}
