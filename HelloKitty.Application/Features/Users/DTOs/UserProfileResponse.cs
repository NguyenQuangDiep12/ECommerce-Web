using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Users.DTOs
{
    public class UserProfileResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly? BirthDay { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
