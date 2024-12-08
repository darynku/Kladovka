using Kladovka.Contracts.Handlers;
using Security.Applications.AppServices.Identity.Services.Abstract;


namespace Security.Applications.Handlers.Identity.Commands.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly IAuthorizationService _authorizationService;

        public RegisterCommandHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
             await _authorizationService.RegisterAsync(
                request.FirstName, 
                request.LastName, 
                request.UserName, 
                request.Password, 
                request.Email, cancellationToken);
            
        }
    }
}
