using Shared.Commands;

namespace Application.Commands.Auth
{
    public class LoginCommand : ICommand
    {
        public string Identity { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
