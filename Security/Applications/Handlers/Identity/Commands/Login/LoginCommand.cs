using Kladovka.Contracts.Abstract;
using Security.Contracts.Identity;
using Security.Contracts.Identity.Response;

namespace Security.Applications.Handlers.Identity.Commands.Login
{
    public class LoginCommand : Command<LoginResponse>
    {
        public LoginCommand(LoginData data)
        {
            UserName = data.UserName;
            Password = data.Password;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
