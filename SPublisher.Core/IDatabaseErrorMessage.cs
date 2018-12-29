namespace SPublisher.Core
{
    public interface IDatabaseErrorMessage : ILogMessage
    {
        string ErrorMessage { get; }
    }
}