﻿using Security.Applications.AppServices.Identity.Services.Abstract;
using Security.Clients.Keycloack;
using Security.Contracts.Identity.Response;

namespace Security.Applications.AppServices.Identity.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IKeycloakAuthorizationApi _keycloack;
    private readonly ILogger<AuthorizationService> _logger;

    public AuthorizationService(IKeycloakAuthorizationApi keycloack, ILogger<AuthorizationService> logger)
    {
        _keycloack = keycloack;
        _logger = logger;
    }

    public async Task<LoginResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken)
    {
        var response = await _keycloack.AuthorizeAsync(username, password, cancellationToken);
        if (response.IsError)
        {
            _logger.LogError($"При авторизации {username} произошла ошибка {response.Error}, {response.ErrorDescription}");
            throw new Exception($"При авторизации {username} произошла ошибка");
        }
        return new LoginResponse(response.AccessToken!, response.RefreshToken!);
    }
}