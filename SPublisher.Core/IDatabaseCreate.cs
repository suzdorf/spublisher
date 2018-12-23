namespace SPublisher.Core
{
    public interface IDatabaseCreate : ILogMessage
    {
        string DbName { get; }
    }
}