using Shared.Commands;

namespace Application.Commands.RentOut
{
    public class CompleteRentCommand : ICommand
    {
        public Guid RentId { get; set; }
        public string Cnpj { get; private set; } = string.Empty;

        public void SetCourierCnpj(string cnpj)
        {
            Cnpj = cnpj;
        }
    }
}
