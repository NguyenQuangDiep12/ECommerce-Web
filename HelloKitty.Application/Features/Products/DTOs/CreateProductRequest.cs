using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.DTOs
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public Guid CategoryId {  get; set; }
        public List<CreateVariantRequest> Variants { get; set; } = new List<CreateVariantRequest>();
    }
}
