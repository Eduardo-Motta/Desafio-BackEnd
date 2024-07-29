namespace Shared.Utils
{
    public sealed class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
