using Shared.Commands;

namespace Domain.Commands
{
    public class CommandResponseError : ICommandResult
    {
        public CommandResponseError(string message)
        {
            Message = message;
            Success = false;
        }

        public string Message { get; }
        public bool Success { get; }
    }
}
