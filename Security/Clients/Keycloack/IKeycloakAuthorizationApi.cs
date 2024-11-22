using IdentityModel.Client;

namespace Security.Clients.Keycloack
{
    public interface IKeycloakAuthorizationApi
    {
        Task<TokenResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken);
    }
}