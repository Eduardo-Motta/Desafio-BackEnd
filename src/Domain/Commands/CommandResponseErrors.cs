using FluentValidation.Results;
using Shared.Commands;

namespace Domain.Commands
{
    public class CommandResponseErrors : ICommandResult
    {
        public CommandResponseErrors(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures.Select(x =>
                new ErrorMessage(
                    x.PropertyName,
                    x.ErrorMessage,
                    x.AttemptedValue?.ToString()));
            Success = false;
        }

        public IEnumerable<ErrorMessage> Errors { get; }
        public bool Success { get; }
    }
}
