using DocumentValidator;
using FluentValidation;

namespace Application.Commands.RentOut.Validations
{
    public class CompleteRentValidation : AbstractValidator<CompleteRentCommand>
    {
        public CompleteRentValidation()
        {
            RuleFor(command => command.RentId)
                .NotEmpty().WithMessage("RentId cannot be empty");

            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .WithMessage("Cnpj is required")
                .Must(x => CnpjValidation.Validate(x))
                .When(x => !string.IsNullOrEmpty(x.Cnpj), ApplyConditionTo.CurrentValidator)
                .WithMessage("Invalid Cnpj");
        }
    }
}
