namespace Security.Contracts.Identity.Response
{
    public record LoginResponse(string AccessToken, string RefreshToken);
}
