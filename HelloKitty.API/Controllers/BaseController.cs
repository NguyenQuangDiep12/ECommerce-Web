using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelloKitty.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected Guid CurrentUserId
        {
            get
            {
                return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) 
                    ?? throw new UnauthorizedAccessException("User not found!"));
            }
        }
    }
}
