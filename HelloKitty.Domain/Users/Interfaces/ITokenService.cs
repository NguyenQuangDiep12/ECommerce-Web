using HelloKitty.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Users.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
