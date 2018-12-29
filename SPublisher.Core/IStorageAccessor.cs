using System.Collections.Generic;

namespace SPublisher.Core
{
    public interface IStorageAccessor
    {
        string ReadAllText(string path);

        IDictionary<string, string> ReadAllText(string folderPath, string extension);
    }
}