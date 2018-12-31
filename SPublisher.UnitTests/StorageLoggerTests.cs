using System;
using Moq;
using SPublisher.Core;
using Xunit;

namespace SPublisher.UnitTests
{
    public class StorageLoggerTests
    {
        private const string LocalFolderPath = "LocalFolderPath";
        private const string LogsFolderPath = LocalFolderPath + "\\logs";
        private const string LogsFilePath = LogsFolderPath + "\\logs.txt";
        private const string ExceptionMessage = "ExceptionMessage";
        private const string ExceptionStackTrace = "EsceptionStackTrace";
        private readonly Mock<IStorageAccessor> _storageAccessorMock = new Mock<IStorageAccessor>();
        private readonly IStorageLogger _storageLogger;
        private readonly Mock<Exception> _exceptionMock =  new Mock<Exception>();

        public StorageLoggerTests()
        {
            _storageLogger = new StorageLogger(_storageAccessorMock.Object, LocalFolderPath);
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 1)]
        public void ShouldCreateLogsFolderIfNotExists(bool isExists, int times)
        {
            _storageAccessorMock.Setup(x => x.DirectoryExists(LogsFolderPath)).Returns(isExists);
            _storageLogger.LogError(_exceptionMock.Object);
            _storageAccessorMock.Verify(x=>x.DirectoryExists(LogsFolderPath), Times.Once);
            _storageAccessorMock.Verify(x => x.CreateDirectory(LogsFolderPath), Times.Exactly(times));
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 1)]
        public void ShouldCreateLogsFileIfNotExists(bool isExists, int times)
        {
            _storageAccessorMock.Setup(x => x.FileExists(LogsFilePath)).Returns(isExists);
            _storageLogger.LogError(_exceptionMock.Object);
            _storageAccessorMock.Verify(x => x.FileExists(LogsFilePath), Times.Once);
            _storageAccessorMock.Verify(x => x.CreateFile(LogsFilePath), Times.Exactly(times));
        }

        [Fact]
        public void ShouldLogMessageAndStackTrace()
        {
            _exceptionMock.SetupGet(x => x.Message).Returns(ExceptionMessage);
            _exceptionMock.SetupGet(x => x.StackTrace).Returns(ExceptionStackTrace);
            _storageLogger.LogError(_exceptionMock.Object);
            _storageAccessorMock.Verify(x => x.AppendAllText(LogsFilePath,
                It.Is<string>(y => y.Contains(ExceptionMessage) && y.Contains(ExceptionStackTrace))));
        }
    }
}