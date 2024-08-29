using Shared.Commands;

namespace Application.Commands.RentOut
{
    public class CreateRentOutCommand : ICommand
    {
        public Guid PlanId { get; set; }
        public Guid MotorcycleId { get; set; }
        public string Cnpj { get; private set; } = string.Empty;
        public DateTime ExpectedEndDate { get; set; }

        public void SetCourierCnpj(string cnpj)
        {
            Cnpj = cnpj;
        }
    }
}
