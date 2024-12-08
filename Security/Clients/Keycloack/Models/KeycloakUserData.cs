namespace Security.Clients.Keycloack.Models
{
    
    public class KeycloakUserData
    {
        public KeycloakUserData(Guid userId)
        {
            Id = userId;
        }
        public Guid Id { get; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFirstLogin  { get; set; }

        public ICollection<KeycloakRolesData> Roles { get; set; }

    }
}