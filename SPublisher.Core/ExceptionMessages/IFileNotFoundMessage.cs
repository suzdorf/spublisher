namespace SPublisher.Core.ExceptionMessages
{
    public interface IFileNotFoundMessage : ILogMessage
    {
        string Path { get; }
    }
}