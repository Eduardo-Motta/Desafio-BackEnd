using Shared.Commands;

namespace Application.Commands.Motorcycle
{
    public class DeleteMotorcycleCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
