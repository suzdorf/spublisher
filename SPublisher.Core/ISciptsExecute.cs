namespace SPublisher.Core
{
    public interface ISciptsExecute
    {
        string DatabaseName { get; }

        string[] Scripts { get; }
    }
}