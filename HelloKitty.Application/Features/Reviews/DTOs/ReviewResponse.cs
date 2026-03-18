using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Reviews.DTOs
{
    public class ReviewResponse
    {
        public int ReviewId { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
