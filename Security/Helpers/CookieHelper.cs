namespace Security.Helpers
{
    public static class CookieHelper
    {
        private static readonly string AccessTokenName = "access-token";
        private static readonly string RefreshTokenName = "refresh-token";
        public static void SetAccessTokenCookie(string accessToken, IResponseCookies cookie)
        {
            var cookieOption = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(10),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };
            cookie.Append(AccessTokenName, accessToken, cookieOption);
        }

        public static void SetRefreshTokenCookie(string refreshToken, IResponseCookies cookie)
        {
            var cookieOption = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };
            cookie.Append(RefreshTokenName, refreshToken, cookieOption);
        }
    }
}
