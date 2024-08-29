using Shared.Commands;

namespace Application.Commands.Motorcycle
{
    public class FindMotorcycleByLicensePlateCommand : ICommand
    {
        public string LicensePlate { get; set; } = string.Empty;
    }
}
