using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Security.Clients.Keycloak.Generated;
using Security.Clients.Options;
using Security.Contracts.Enums;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

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
                Policy = { RequireHttps =  _options.RequireHttpsMetadata }
            }, cancellationToken);
        }
        public async Task<object> RegisterUser(
            string firstName,
            string lastName,
            string username,
            string password,
            string email,
            CancellationToken cancellationToken)
        {
            try
            {
                // Создание объекта пользователя
                var userRepresentation = new
                {
                    username,
                    email,
                    enabled = true,
                    firstName,
                    lastName,
                    credentials = new[]
                    {
                        new
                        {
                            type = "password",
                            value = password,
                            temporary = false
                        }
                    }
                };

                var client = _httpClientFactory.CreateClient();

                // Получение токена администратора
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.ApiAdminBaseUrl}/realms/{_options.Realm}/protocol/openid-connect/token")
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "client_id", _options.ClientId },
                        { "client_secret", _options.ClientSecret },
                        { "grant_type", "client_credentials" }
                    })
                };

                var tokenResponse = await client.SendAsync(tokenRequest, cancellationToken);
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    var error = await tokenResponse.Content.ReadAsStringAsync();
                    _logger.LogError($"Ошибка при получении токена администратора: {error}");
                    throw new Exception("Не удалось получить токен администратора.");
                }


                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<JObject>(tokenContent)?["access_token"]?.ToString();

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Токен администратора пуст или не получен.");
                }

                // Отправка запроса на создание пользователя
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(userRepresentation),
                    Encoding.UTF8,
                    "application/json");

                var createUserResponse = await client.PostAsync($"{_options.ApiAdminBaseUrl}/admin/realms/{_options.Realm}/users", jsonContent, cancellationToken);

                if (!createUserResponse.IsSuccessStatusCode)
                {
                    var errorResponse = await createUserResponse.Content.ReadAsStringAsync();
                    _logger.LogError($"Не удалось зарегистрировать пользователя {username}. Ошибка: {errorResponse}");
                    throw new Exception($"Ошибка при регистрации пользователя {username}: {errorResponse}");
                }

                _logger.LogInformation($"Пользователь {username} успешно зарегистрирован в Keycloak.");
                return userRepresentation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя {Username}.", username);
                throw;
            }
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
                    Password = password,
                    Scope = "openid"
                };

                using var httpClient = _httpClientFactory.CreateClient();
                var discoveryDocumentResponse = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);
                if (discoveryDocumentResponse is null)
                {
                    throw new Exception($"Нет ссылки на авторизацию {discoveryDocumentResponse?.Error}");
                }

                request.RequestUri = new Uri(discoveryDocumentResponse.TokenEndpoint!);
                response = await httpClient.RequestPasswordTokenAsync(request, cancellationToken);

                if (response.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError(
                        "Не удалось авторизовать пользователя: {UserName}. Ошибка: {Error} ({ErrorDescription})",
                        username, response.Error, response.ErrorDescription);

                    throw new Exception($"Введен неверный логин или пароль.");
                }

                if (response.IsError)
                {
                    _logger.LogError(
                        "Не удалось авторизовать пользователя: {UserName}. Ошибка: {Error} ({ErrorDescription})",
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
            
            var roles = await _generatedApi.RolesAll2Async(_options.Realm, cancellationToken: cancellationToken);
            if (roles == null || roles.Count == 0)
                throw new Exception("Нету ролей для этого realma");

            // Получаем текущего пользователя.
            var users = await _generatedApi.UsersAll3Async(_options.Realm, username: username, max: 1, cancellationToken: cancellationToken);
            var user = users.FirstOrDefault();
            if (user is null)
            {
                return;
            }
            var userIdParam = user.Id!.ToString();

            // Текущие роли пользователя.
            var userRoles = await _generatedApi.ClientsAll9Async(_options.Realm, userIdParam, "8dd4b223-fb05-4e7c-8896-c17bd502ed21", cancellationToken: cancellationToken);
            // Если роль уже есть, то ничего не делаем
            if (userRoles?.Count != 0)
            {
                return;
            }

            // Ролей нет, поэтому добавляем default роль - Клиент.
            var availableRoles = await _generatedApi.Available9Async(_options.Realm, userIdParam, "8dd4b223-fb05-4e7c-8896-c17bd502ed21", cancellationToken);
            var defaultRole = availableRoles.FirstOrDefault(r => r.Name == RoleTypes.Client.ToString());
            if (defaultRole is null)
            {
                return;
            }
            await _generatedApi.ClientsPOST6Async(_options.Realm, userIdParam, "5f5a1caf-af31-45d8-aab1-60050d82957a", [defaultRole], cancellationToken);
        }


    }
}
