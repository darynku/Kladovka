using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Security.Applications.Handlers.Identity.Commands.Login;
using Security.Contracts.Identity;
using Security.Helpers;

namespace Security.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginData data, CancellationToken cancellationToken)
        {
            var request = new LoginCommand(data);
            var result = await _mediator.Send(request, cancellationToken);
            CookieHelper.SetAccessTokenCookie(result.AccessToken, Response.Cookies);
            CookieHelper.SetRefreshTokenCookie(result.RefreshToken, Response.Cookies);
            return Ok(result.AccessToken);
        }
    }
}
