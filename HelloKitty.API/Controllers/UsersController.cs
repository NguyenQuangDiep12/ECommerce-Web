using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using HelloKitty.Application.Features.Auth;
using Microsoft.AspNetCore.Identity;

namespace HelloKitty.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : BaseController
    {
        
    }
}
