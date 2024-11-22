using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Security.Clients.Keycloak.Generated;
using Security.Clients.Options;
using Security.Contracts.Enums;

namespace Security.Clients.Keycloack
{
    public class KeycloakAuthorizationApi : IKeycloakAuthorizationApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly KeycloakOptions _options;
        private readonly ILogger<KeycloakAuthorizationApi> _logger;
        private readonly IKeycloakGeneratedExternalApiClient _generatedApi;
        public KeycloakAuthorizationApi(
            IHttpClientFactory httpClientFactory,
            IOptions<KeycloakOptions> options,
            ILogger<KeycloakAuthorizationApi> logger,
            IKeycloakGeneratedExternalApiClient generatedApi)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _logger = logger;
            _generatedApi = generatedApi;
        }
        private Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient client, CancellationToken cancellationToken)
        {
            return client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _options.MetadataAddress,
                Policy = { RequireHttps =  false }
            }, cancellationToken);
        }

        public async Task<TokenResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken)
        {
            TokenResponse response;
            try
            {
                var request = new PasswordTokenRequest
                {
                    ClientId = _options.ClientId,
                    ClientSecret = _options.ClientSecret,
                    GrantType = OidcConstants.GrantTypes.Password,
                    Method = HttpMethod.Post,
                    UserName = username,
                    Password = password
                };

                using var httpClient = _httpClientFactory.CreateClient();
                var discoveryDocumentResponse = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);
                if (discoveryDocumentResponse is null)
                {
                    throw new Exception($"Нет ссылки на авторизацию {discoveryDocumentResponse?.Error}");
                }

                request.RequestUri = new Uri(discoveryDocumentResponse.TokenEndpoint);
                response = await httpClient.RequestPasswordTokenAsync(request, cancellationToken);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Не удалось авторизовать пользователя: {UserName}. Ошибка: {Error} ({ErrorDescription})",
                        username, response.Error, response.ErrorDescription);
                    throw new Exception($"Введен неверный логин или пароль.");
                }

                if (response.IsError)
                {
                    _logger.LogError("Не удалось авторизовать пользователя: {UserName}. Ошибка: {Error} ({ErrorDescription})",
                        username, response.Error, response.ErrorDescription);
                    throw new Exception($"Не удалось авторизовать пользователя: {username}.");
                }

                await SetDefaultRoleAsync(username, cancellationToken);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "При авторизации {UserName} произошла ошибка", username);
                throw new Exception($"Произошла ошибка при авторизации {ex.Message}");
            }
            return response;
        }

        private async Task SetDefaultRoleAsync(string username, CancellationToken cancellationToken)
        {
            // Получаем текущего пользователя.
            var users = await _generatedApi.UsersAll3Async(_options.Realm, enabled: true, username: username, max: 1, cancellationToken: cancellationToken);
            var user = users.FirstOrDefault();
            if (user is null)
            {
                return;
            }
            var userIdParam = user.Id!.ToString();

            // Текущие роли пользователя.
            var userRoles = await _generatedApi.ClientsAll9Async(_options.Realm, userIdParam, _options.ClientId, cancellationToken);
            // Если роль уже есть, то ничего не делаем
            if (userRoles?.Count != 0)
            {
                return;
            }

            // Ролей нет, поэтому добавляем default роль - Клиент.
            var availableRoles = await _generatedApi.Available9Async(_options.Realm, userIdParam, _options.ClientId.ToString(), cancellationToken);
            var defaultRole = availableRoles.FirstOrDefault(r => r.Name == RoleTypes.Client.ToString());
            if (defaultRole is null)
            {
                return;
            }
            await _generatedApi.ClientsPOST6Async(_options.Realm, userIdParam, _options.ClientId, [defaultRole], cancellationToken);
        }


    }
}
