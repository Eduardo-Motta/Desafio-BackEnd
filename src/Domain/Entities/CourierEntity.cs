using Domain.Enums;

namespace Domain.Entities
{
    public class CourierEntity : BaseEntity
    {
        public CourierEntity(string name, string cnpj, DateTime birthDate, string drivingLicense, EDrivingLicenseCategory drivingLicenseCategory)
        {
            Name = name;
            Cnpj = cnpj;
            BirthDate = birthDate;
            DrivingLicense = drivingLicense;
            DrivingLicenseCategory = drivingLicenseCategory;
        }

        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string DrivingLicense { get; private set; }
        public EDrivingLicenseCategory DrivingLicenseCategory { get; private set; }
        public string DrivingLicensePath { get; private set; } = string.Empty;

        public void SetDrivingLicensePath(string drivingLicensePath)
        {
            DrivingLicensePath = drivingLicensePath;
            UpdatedAt = DateTime.Now;
        }
    }
}
