using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Users.DTOs
{
    public class CreateAddressRequest
    {
        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
