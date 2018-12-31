using System;
using System.Globalization;
using SPublisher.Core;

namespace SPublisher
{
    public class StorageLogger : IStorageLogger
    {
        private readonly IStorageAccessor _storageAccessor;
        private readonly string _logsFolderPath;
        private readonly string _logsFilePath;

        public StorageLogger(IStorageAccessor storageAccessor, string localFolderPath)
        {
            _storageAccessor = storageAccessor;
            _logsFolderPath = $"{localFolderPath}\\logs";
            _logsFilePath = $"{localFolderPath}\\logs\\logs.txt";
        }

        public void LogError(Exception exception)
        {
            if (!_storageAccessor.DirectoryExists(_logsFolderPath))
            {
                _storageAccessor.CreateDirectory(_logsFolderPath);
            }

            if (!_storageAccessor.FileExists(_logsFilePath))
            {
                _storageAccessor.CreateFile(_logsFilePath);
            }

            var currentTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var message = exception.Message;
            var stackTrace = exception.StackTrace;

            _storageAccessor.AppendAllText(_logsFilePath,
                $"{Environment.NewLine}{currentTime}{Environment.NewLine}Message: {message}{Environment.NewLine}StackTrace:{stackTrace}");
        }
    }
}