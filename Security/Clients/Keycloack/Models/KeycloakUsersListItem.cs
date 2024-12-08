namespace Security.Clients.Keycloack.Models
{
    public class KeycloakUsersListItem
    {
        public KeycloakUsersListItem(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<KeycloakRolesData> Roles { get; set; }
    }
}