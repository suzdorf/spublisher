namespace SPublisher.Core.ExceptionMessages
{
    public interface IDirectoryNotFoundMessage : ILogMessage
    {
        string Path { get; }
    }
}