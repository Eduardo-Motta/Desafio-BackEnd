namespace Domain.Events
{
    public class MotorcycleRegisteredEvent
    {
        public Guid Id { get; }
        public int Year { get; }
        public string Model { get; }
        public string LicensePlate { get; }

        public MotorcycleRegisteredEvent(Guid id, int year, string model, string licensePLate)
        {
            Id = id;
            Year = year;
            Model = model;
            LicensePlate = licensePLate;
        }
    }
}
