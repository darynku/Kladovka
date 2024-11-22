using System.ComponentModel;

namespace Security.Contracts.Enums
{
    public enum RoleTypes 
    {
        Underfiend,

        [Description("Админ")]
        Administrator,
        
        [Description("Клиент")]
        Client
    }
}
