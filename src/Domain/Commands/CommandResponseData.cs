using Shared.Commands;

namespace Domain.Commands
{
    public class CommandResponseData<T> : ICommandResult
    {
        public CommandResponseData(T data)
        {
            Data = data;
            Success = true;
        }

        public T Data { get; set; }
        public bool Success { get; }
    }
}
