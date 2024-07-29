using Shared.Commands;

namespace Shared.Handlers
{
    public interface IHandler<T> where T : ICommand
    {
        Task<ICommandResult> Handle(T command, CancellationToken cancellationToken);
    }
}