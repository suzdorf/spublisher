using System.Collections.Generic;

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
        IDictionary<string, string> ReadAllText(string folderPath, string extension);
    }
}