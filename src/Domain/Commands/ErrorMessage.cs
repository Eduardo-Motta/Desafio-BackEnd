namespace Domain.Commands
{
    public class ErrorMessage
    {
        public ErrorMessage(string property, string message, string? attemptedValue = null)
        {
            Message = message;
            Property = property;
            AttemptedValue = attemptedValue;
        }

        public string Message { get; }
        public string Property { get; }
        public string? AttemptedValue { get; } = null;
    }
}
