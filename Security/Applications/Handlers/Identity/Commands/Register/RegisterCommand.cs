using Kladovka.Contracts.Abstract;
using Security.Contracts.Identity;

namespace Security.Applications.Handlers.Identity.Commands.Register
{
    public class RegisterCommand: Command<string>
    {
        public RegisterCommand(RegisterData data)
        {
            FirstName = data.FirstName;
            LastName = data.LastName;
            UserName = data.UserName;
            Password = data.Password;
            Email = data.Email; 
        }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; }
        public string Email { get; set; } 
    };
}
