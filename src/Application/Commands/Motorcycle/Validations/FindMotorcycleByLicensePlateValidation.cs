using FluentValidation;

namespace Application.Commands.Motorcycle.Validations
{
    public class FindMotorcycleByLicensePlateValidation : AbstractValidator<FindMotorcycleByLicensePlateCommand>
    {
        public FindMotorcycleByLicensePlateValidation()
        {
            RuleFor(x => x.LicensePlate)
                .NotEmpty()
                .WithMessage("LicensePlate is required")
                .Must(x => new LicensePlateValidation().Validate(x))
                .When(x => !string.IsNullOrEmpty(x.LicensePlate), ApplyConditionTo.CurrentValidator)
                .WithMessage("Invalid LicensePlate");
        }
    }
}
