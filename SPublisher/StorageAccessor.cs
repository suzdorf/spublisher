using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using SPublisher.Configuration.Models;
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

        public IFile GetFile(string path)
        {
            return new FileModel
            {
                Path = Path.GetFullPath(path),
                Content = ReadAllText(path),
                Hash = CalculateHash(path)
            };
        }

        public IFile[] GetFiles(string folderPath, string extension)
        {
            var path = Path.GetFullPath(folderPath);

            if (Directory.Exists(path))
            {
                return Directory.EnumerateFiles(path, $"*{extension}").Select(GetFile).ToArray();
            }

            throw new Core.Exceptions.DirectoryNotFoundException(folderPath);
        }

        private static string CalculateHash(string path)
        {
            using (var md5 = SHA256.Create())
            {
                using (var stream = File.OpenRead(Path.GetFullPath(path)))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}