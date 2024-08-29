using Shared.Commands;

namespace Application.Commands.RentOut
{
    public class SimulateRentOutCommand : ICommand
    {
        public Guid PlanId { get; set; }
        public Guid MotorcycleId { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public string Cnpj { get; private set; } = string.Empty;

        public void SetCourierCnpj(string cnpj)
        {
            Cnpj = cnpj;
        }
    }
}
