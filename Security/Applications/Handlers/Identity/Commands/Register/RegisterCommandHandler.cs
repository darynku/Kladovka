using Kladovka.Contracts.Handlers;
using Security.Applications.AppServices.Identity.Services.Abstract;


namespace Security.Applications.Handlers.Identity.Commands.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, string>
    {
        private readonly IAuthorizationService _authorizationService;

        public RegisterCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = await _authorizationService.RegisterAsync(
                request.FirstName, 
                request.LastName, 
                request.UserName, 
                request.Password, 
                request.Email, cancellationToken);

            return response;
        }
    }
}
