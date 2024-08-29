using Shared.Commands;
using System.Runtime.Serialization;

namespace Application.Commands.Courier
{
    public enum EDrivingLicenseCategory
    {
        [EnumMember(Value = "A")]
        A,
        [EnumMember(Value = "B")]
        B,
        [EnumMember(Value = "AB")]
        AB
    }

    public class CreateCourierCommand : ICommand
    {
        public string Name { get; set; } = String.Empty;
        public string Cnpj { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public DateTime BirthDate { get; set; }
        public string DrivingLicense { get; set; } = String.Empty;
        public EDrivingLicenseCategory DrivingLicenseCategory { get; set; }
    }
}
