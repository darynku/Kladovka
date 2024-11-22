namespace Security.Contracts.Identity
{
    public class LoginData
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
