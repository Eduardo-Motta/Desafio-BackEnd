using FluentValidation;

namespace Application.Commands.Auth.Validations
{
    public class LoginValidation : AbstractValidator<LoginCommand>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Identity)
                .NotEmpty()
                .WithMessage("Identity is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
