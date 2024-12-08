namespace Security.Clients.Keycloack.Models.Requests
{
    public class KeycloakUserRequest
    {
        public string userName { get; set; }
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public ICollection<Credentials> Credentials { get; set; }
    }

    public class Credentials
    {
        public static string PasswordType = "password";
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Temporary { get; set; }
    }
}
