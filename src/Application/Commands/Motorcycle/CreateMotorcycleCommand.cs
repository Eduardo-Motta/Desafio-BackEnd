using Shared.Commands;

namespace Application.Commands.Motorcycle
{
    public class CreateMotorcycleCommand : ICommand
    {
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
    }
}
