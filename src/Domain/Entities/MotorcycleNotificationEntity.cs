namespace Domain.Entities
{
    public class MotorcycleNotificationEntity
    {
        public MotorcycleNotificationEntity(Guid id, string model, int year, string licensePlate)
        {
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
            Id = id;
        }

        public int Year { get; private set; }
        public string Model { get; private set; }
        public string LicensePlate { get; private set; }
        public Guid Id { get; protected set; }
    }
}
