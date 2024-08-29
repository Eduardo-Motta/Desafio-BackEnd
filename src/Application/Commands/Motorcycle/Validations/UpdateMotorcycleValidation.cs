using FluentValidation;

namespace Application.Commands.Motorcycle.Validations
{
    public class UpdateMotorcycleValidation : AbstractValidator<UpdateMotorcycleCommand>
    {
        public UpdateMotorcycleValidation()
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
