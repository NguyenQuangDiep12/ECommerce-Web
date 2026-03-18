using HelloKitty.Domain.Catalog.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.DTOs
{
    public class ProductDetailResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public ProductStatus Status { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImageResponse> Images { get; set; } = new List<ProductImageResponse>();
        public List<VariantResponse> Variants { get; set; } = new List<VariantResponse>();
    }
}
