namespace SPublisher.Core.DbManagement
{
    public interface IScripts
    {
        bool IsFolder { get; }
        string Path { get; }
    }
}