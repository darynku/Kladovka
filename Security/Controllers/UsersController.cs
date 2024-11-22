using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Security.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("me")]
        [Authorize]
        public IActionResult CurrentUser()
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
            return Ok(claims);
        }
    }
}
