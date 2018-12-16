namespace SPublisher.Core
{
    public interface IStorageAccessor
    {
        bool CheckDirectoryExists(string path);
    }
}