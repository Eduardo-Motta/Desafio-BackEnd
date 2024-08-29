namespace Domain.Entities
{
    public class MotorcycleEntity : BaseEntity
    {
        public MotorcycleEntity(int year, string model, string licensePlate)
        {
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
        }

        public int Year { get; private set; }
        public string Model { get; private set; }
        public string LicensePlate { get; private set; }

        public void Update(string licensePlate)
        {
            LicensePlate = licensePlate;
            UpdatedAt = DateTime.Now;
        }
    }
}
