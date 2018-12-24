using System.IO;
using SPublisher.Core;

namespace SPublisher
{
    public class StorageAccessor : IStorageAccessor
    {
        public bool CheckDirectoryExists(string path)
        {
            throw new System.NotImplementedException();
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
    }
}