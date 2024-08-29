using DocumentValidator;
using FluentValidation;

namespace Application.Commands.Courier.Validations
{
    public class CreateCourierValidation : AbstractValidator<CreateCourierCommand>
    {
        public CreateCourierValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .WithMessage("Cnpj is required")
                .Must(x => CnpjValidation.Validate(x))
                .When(x => !string.IsNullOrEmpty(x.Cnpj), ApplyConditionTo.CurrentValidator)
                .WithMessage("Invalid Cnpj");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(8)
                .WithMessage("The password must be at least 8 characters long");

            RuleFor(x => x.DrivingLicense)
                .NotEmpty()
                .WithMessage("DrivingLicense is required")
                .Must(x => CnhValidation.Validate(x))
                .When(x => !string.IsNullOrEmpty(x.DrivingLicense), ApplyConditionTo.CurrentValidator)
                .WithMessage("Invalid DrivingLicense");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage("BirthDate is required")
                .LessThanOrEqualTo(DateTime.Now.Date.AddYears(-18))
                .WithMessage("Driver must be 18 years or older");

            RuleFor(x => x.DrivingLicenseCategory)
                .IsInEnum()
                .WithMessage("Invalid DrivingLicenseCategory");
        }
    }
}
