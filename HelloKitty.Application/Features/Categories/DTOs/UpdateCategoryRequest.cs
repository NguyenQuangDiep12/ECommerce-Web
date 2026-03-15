using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Categories.DTOs
{
    public class UpdateCategoryRequest
    {
        public string CategoryName { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
    }
}
