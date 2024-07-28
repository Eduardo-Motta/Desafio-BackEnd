using FluentValidation.Results;
using Shared.Commands;

namespace Domain.Commands
{
    public class CommandResponseError : ICommandResult
    {
        public CommandResponseError(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures.Select(x =>
                new ErrorMessage(
                    x.PropertyName,
                    x.ErrorMessage,
                    x.AttemptedValue?.ToString()));
            Success = true;
        }

        public IEnumerable<ErrorMessage> Errors { get; }
        public bool Success { get; }
    }
}
