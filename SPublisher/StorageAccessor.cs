using System.Collections.Generic;
using System.IO;
using System.Linq;
using SPublisher.Core;

namespace SPublisher
{
    public class StorageAccessor : IStorageAccessor
    {
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(Path.GetFullPath(path));
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(Path.GetFullPath(path));
        }

        public bool FileExists(string path)
        {
            return File.Exists(Path.GetFullPath(path));
        }

        public void CreateFile(string path)
        {
            File.Create(Path.GetFullPath(path)).Dispose();
        }

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

        public void AppendAllText(string path, string text)
        {
            File.AppendAllText(Path.GetFullPath(path), text);
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