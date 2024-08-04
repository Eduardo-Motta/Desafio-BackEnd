using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Domain.Commands.Courier.Validations
{
    public class UploadDrivingLicenseValidation : AbstractValidator<UploadDrivingLicenseCommand>
    {
        public UploadDrivingLicenseValidation()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("The file is required.")
                .Must(BeAValidFileType).WithMessage("Only PNG or BMP files are allowed.");
        }

        private bool BeAValidFileType(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }

            var allowedExtensions = new[] { ".png", ".bmp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
