using Shared.Commands;

namespace Domain.Commands
{
    public class CommandResponseCreated : ICommandResult
    {
        public CommandResponseCreated(Guid id)
        {
            Id = id;
            Success = true;
        }

        public Guid Id { get; }
        public bool Success { get; }
    }
}
