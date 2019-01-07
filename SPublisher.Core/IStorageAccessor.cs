namespace SPublisher.Core
{
    public interface IStorageAccessor
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        bool FileExists(string path);
        void CreateFile(string path);
        string ReadAllText(string path);
        void AppendAllText(string path, string text);
        IFile GetFile(string path);
        IFile[] GetFiles(string folderPath, string extension);
    }
}