using HelloKitty.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Users.Entities
{
    public class RefreshToken
    {
        public Guid TokenId { get; private set; } = Guid.NewGuid();
        public string Token { get;  set; } = string.Empty;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } 
        public DateTime? RevokedAt { get; set; }
        public Guid? CreatedById { get; set; }
        public bool IsActive
        {
            get
            {
                return !IsRevoked && DateTime.UtcNow < ExpiresAt;
            }
        }
    }
}
