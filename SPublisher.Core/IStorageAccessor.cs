namespace SPublisher.Core
{
    public interface IStorageAccessor
    {
        bool CheckDirectoryExists(string path);

        string ReadAllText(string path);
    }
}