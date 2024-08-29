using Shared.Commands;

namespace Application.Commands.Motorcycle
{
    public class UpdateMotorcycleCommand : ICommand
    {
        private Guid _motorcycleId;

        public string LicensePlate { get; set; } = string.Empty;

        public void SetMotorcycleId(Guid id)
        {
            _motorcycleId = id;
        }

        public Guid getMotorcycleId()
        {
            return _motorcycleId;
        }
    }
}
