namespace Security.Clients.Keycloack.Models
{
    public class KeycloakRolesData
    {
        public KeycloakRolesData(string id)
        {
            Id = id;
        }
        public string Id { get; }
        public string Name { get; set; } = string.Empty;
    }
}
