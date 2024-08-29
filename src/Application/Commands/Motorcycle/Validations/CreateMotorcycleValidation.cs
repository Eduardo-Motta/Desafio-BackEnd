using FluentValidation;

namespace Application.Commands.Motorcycle.Validations
{
    public class CreateMotorcycleValidation : AbstractValidator<CreateMotorcycleCommand>
    {
        public CreateMotorcycleValidation()
        {
            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model is required");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(2010)
                .WithMessage("Year must be greater than or equal to 2010");

            RuleFor(x => x.LicensePlate)
                .NotEmpty()
                .WithMessage("LicensePlate is required")
                .Must(x => new LicensePlateValidation().Validate(x))
                .When(x => !string.IsNullOrEmpty(x.LicensePlate), ApplyConditionTo.CurrentValidator)
                .WithMessage("Invalid LicensePlate");
        }
    }
}
