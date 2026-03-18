using HelloKitty.Domain.Catalog.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.DTOs
{
    public class ProductListResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public ProductStatus Status { get; set; }
        public string CategoryName { get; set; }
        public string? PrimaryImageUrl { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
