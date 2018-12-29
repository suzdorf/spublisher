using System.Collections.Generic;
using System.IO;
using System.Linq;
using SPublisher.Core;

namespace SPublisher
{
    public class StorageAccessor : IStorageAccessor
    {
        public string ReadAllText(string path)
        {
            try
            {
                return File.ReadAllText(Path.GetFullPath(path));
            }
            catch (FileNotFoundException)
            {
                throw new Core.Exceptions.FileNotFoundException(path);
            }
        }

        public IDictionary<string, string> ReadAllText(string folderPath, string extension)
        {
            var path = Path.GetFullPath(folderPath);
            if (Directory.Exists(path))
            {
                return Directory.EnumerateFiles(path, $"*{extension}").ToDictionary(x => x, File.ReadAllText);
            }

            throw new Core.Exceptions.DirectoryNotFoundException(folderPath);
        }
    }
}