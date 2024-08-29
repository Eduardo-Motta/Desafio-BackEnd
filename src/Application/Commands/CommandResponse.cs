using Shared.Commands;

namespace Application.Commands
{
    public class CommandResponse : ICommandResult
    {
        public CommandResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
    }
}
