using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Reviews.DTOs
{
    public class CreateReviewRequest
    {
        public Guid ProductId { get; set; }
        public decimal Ratings { get; set; }
        public string? Comments { get; set; }
    }
}
