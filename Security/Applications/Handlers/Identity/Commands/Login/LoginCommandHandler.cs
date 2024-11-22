using Kladovka.Contracts.Handlers;
using Security.Applications.AppServices.Identity.Services.Abstract;
using Security.Contracts.Identity.Response;

namespace Security.Applications.Handlers.Identity.Commands.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthorizationService _authorizationService;

        public LoginCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = await _authorizationService.AuthorizeAsync(request.UserName, request.Password, cancellationToken);
            return response;
        }
    }
}
