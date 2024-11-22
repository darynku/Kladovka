using Security.Contracts.Identity.Response;

namespace Security.Applications.AppServices.Identity.Services.Abstract
{
    public interface IAuthorizationService
    {
        Task<LoginResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken);
    }
}