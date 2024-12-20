﻿using IdentityModel.Client;
using Security.Clients.Keycloack.Models.Requests;
using Security.Clients.Keycloak.Generated;

namespace Security.Clients.Keycloack
{
    public interface IKeycloakAuthorizationApi
    {
        Task<TokenResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken);
        Task RegisterUser(string firstName, string lastName, string username, string password, string email, CancellationToken cancellationToken);
    }
}