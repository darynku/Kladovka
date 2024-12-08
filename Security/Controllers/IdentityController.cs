using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Security.Applications.Handlers.Identity.Commands.Login;
using Security.Applications.Handlers.Identity.Commands.Register;
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
        [AllowAnonymous]
        [ProducesDefaultResponseType(typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(LoginData data, CancellationToken cancellationToken)
        {
            var request = new LoginCommand(data);
            var result = await _mediator.Send(request, cancellationToken);
            CookieHelper.SetAccessTokenCookie(result.AccessToken, Response.Cookies);
            CookieHelper.SetRefreshTokenCookie(result.RefreshToken, Response.Cookies);
            return Ok(result.AccessToken);
        }


        [HttpPost("registration")]
        [AllowAnonymous]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterData data, CancellationToken cancellationToken)
        {
            var request = new RegisterCommand(data);
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
