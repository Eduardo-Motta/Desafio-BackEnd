using Domain.Enums;
using Shared.Commands;

namespace Domain.Commands.Courier
{
    public class CreateCourierCommand : ICommand
    {
        public string Name { get; set; } = String.Empty;
        public string Cnpj { get; set; } = String.Empty;
        public DateTime BirthDate { get; set; }
        public string DrivingLicense { get; set; } = String.Empty;
        public EDrivingLicenseCategory DrivingLicenseCategory { get; set; }
    }
}
