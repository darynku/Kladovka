using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Security.Clients.Keycloack.Attributes;
using Security.Clients.Keycloack.Models;
using Security.Clients.Keycloak.Generated;
using Security.Clients.Options;

namespace Security.Clients.Keycloack
{
    public class KeycloakUserApi
    {
        private readonly KeycloakOptions _options;
        private readonly IKeycloakGeneratedExternalApiClient _generatedApiClient;
        private readonly ILogger<KeycloakUserApi> _logger;
        public KeycloakUserApi(
            IOptions<KeycloakOptions> options,
            IKeycloakGeneratedExternalApiClient externalApiClient,
            ILogger<KeycloakUserApi> logger)
        {
            _options = options.Value;
            _generatedApiClient = externalApiClient;
            _logger = logger;
        }

        public async Task<KeycloakUserData?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await GetKeycloakUserAsync(userId, cancellationToken);
            if (user is null)
            {
                return null; 
            }

            var userRoles = await _generatedApiClient.ClientsAll9Async(_options.Realm, user.Id.ToString(), _options.ClientUuid, cancellationToken);
            var userLoginEvents = await _generatedApiClient.EventsAllAsync(_options.Realm, _options.ClientId, null, null, null, null, null, ["LOGIN", "CLIENT_LOGIN"], user.Id.ToString(), cancellationToken);

            return MapToUserResponse(userId, user, userRoles, userLoginEvents);
        }
        private async Task<UserRepresentation> GetKeycloakUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _generatedApiClient.UsersGET2Async(_options.Realm, userId.ToString(), cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting user representation from keycloak: {ex.Message}", ex);
                throw new Exception($"Ошибка при получении пользователя по id: {ex.Message}", ex);
            }
        }

        public async Task<IReadOnlyCollection<KeycloakUsersListItem>> GetUsersByQueryAsync(string query, int take, int? skip, CancellationToken cancellationToken)
        {
            var users = await GetUsersByQueryInternalAsync(query, take, skip, cancellationToken);
            if (users is null || users.Count() == 0)
            {
                return [];
            }

            var roles = await GetUsersRolesAsync(users, cancellationToken);

            return users.Select(u => MapUsersListItemResponse(Guid.Parse(u.Id!), u, roles[u.Id!])).ToArray();
        }

        private async Task<Dictionary<string, ICollection<RoleRepresentation>>> GetUsersRolesAsync(
            ICollection<UserRepresentation> users, CancellationToken cancellationToken)
        { 
            var roles = new Dictionary<string, ICollection<RoleRepresentation>>();
            foreach (var user in users)
            {
                try
                {
                    var userRoles = await _generatedApiClient.ClientsAll9Async(_options.Realm, user.Id!.ToString(), _options.ClientUuid, cancellationToken);
                    roles.Add(user.Id, userRoles);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while getting roles: {UserId}, {Message}", user.Id, ex.Message);
                    throw new Exception(ex.Message, ex);
                }
            }
            return roles;
        }
        private async Task<ICollection<UserRepresentation>> GetUsersByQueryInternalAsync(string query, int take, int? skip, CancellationToken cancellationToken)
        {
            try
            {
                var search = string.Empty;
                if (!string.IsNullOrEmpty(query))
                {
                    search = $"*{string.Join("* *", query.Split(" "))}*";
                }

                var users = await _generatedApiClient.UsersAll3Async(_options.Realm, search: search, max: take, cancellationToken: cancellationToken);
                return users;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message + ": {Query}", query);
                throw new Exception(ex.Message, ex);
            }
        }
        private static KeycloakUserData MapToUserResponse(Guid userId, UserRepresentation userData, ICollection<RoleRepresentation> roles, IEnumerable<EventRepresentation> events)
        {
            return new KeycloakUserData(userId)
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                IsEmailVerified = userData.EmailVerified ?? false,
                IsEnabled = userData.Enabled ?? false,
                Email = userData.Email,
                Roles = roles.Select(r => new KeycloakRolesData(r.Id!) { Name = r.Name! }).ToArray(),
                IsFirstLogin = events.Count() == 1
            };
        }

        private static KeycloakUsersListItem MapUsersListItemResponse(Guid userId, UserRepresentation userData, ICollection<RoleRepresentation> roles)
        {
            return new KeycloakUsersListItem(userId)
            {
                Email = userData.Email!,
                FirstName = userData.FirstName!,
                LastName = userData.LastName!,
                IsEnabled = userData.Enabled ?? false,
                Roles = roles.Select(r => new KeycloakRolesData(r.Id!) { Name = r.Name! }).ToArray()
            };
        }

    }
}
