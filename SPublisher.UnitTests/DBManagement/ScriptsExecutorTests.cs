using Moq;
using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.DbManagement;
using SPublisher.Core.Enums;
using SPublisher.Core.Log;
using SPublisher.DBManagement;
using Xunit;

namespace SPublisher.UnitTests.DBManagement
{
    public class ScriptsExecutorTests
    {
        private const string DatabaseName = "DatabaseName";
        private const string ScriptPath = "ScriptPath";
        private const string Script = "Script";
        private readonly IFile File = new FileModel {Path = ScriptPath, Content = Script};
        private readonly Mock<ISqlServerDataProvider> _sqlServerDataProviderMock = new Mock<ISqlServerDataProvider>();
        private readonly Mock<ISqlServerDataProviderFactory> _sqlServerDataProviderFactoryMock = new Mock<ISqlServerDataProviderFactory>();
        private readonly Mock<IStorageAccessor> _storageAccessorMock = new Mock<IStorageAccessor>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IDatabase> _databaseMock = new Mock<IDatabase>();
        private readonly Mock<IScripts> _scriptsMock = new Mock<IScripts>();
        private readonly IScriptsExecutor _scriptsExecutor;

        public ScriptsExecutorTests()
        {
            _scriptsMock.SetupGet(x => x.Path).Returns(ScriptPath);
            _storageAccessorMock.Setup(x => x.GetFile(ScriptPath))
                .Returns(File);
            _databaseMock.SetupGet(x => x.Scripts).Returns(new[] {_scriptsMock.Object});
            _sqlServerDataProviderFactoryMock.Setup(x => x.Get()).Returns(_sqlServerDataProviderMock.Object);
            _scriptsExecutor = new ScriptsExecutor(_sqlServerDataProviderFactoryMock.Object, _storageAccessorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldExecuteScriptUsingDatabaseName()
        {
            _databaseMock.SetupGet(x => x.DatabaseName).Returns(DatabaseName);
             _scriptsExecutor.ExecuteScripts(_databaseMock.Object);
            _sqlServerDataProviderMock.Verify(x => x.ExecuteScript(Script, DatabaseName), Times.Once);
            _loggerMock.Verify(x =>
                x.LogEvent(SPublisherEvent.SqlScriptExecuted, It.Is<ISqlScriptInfo>(y => y.Path == ScriptPath)), Times.Once);
        }

        [Fact]
        public void ShouldExecuteFilesInFolder()
        {
            var scriptsFromFolder = new IFile[]
            {
                new FileModel{ Path="firstScriptPath", Content = "firstScriptText"},
                new FileModel{ Path="secondScriptPath", Content = "secondScriptText"}
            };
            _databaseMock.SetupGet(x => x.DatabaseName).Returns(DatabaseName);
            _scriptsMock.SetupGet(x => x.IsFolder).Returns(true);
            _storageAccessorMock.Setup(x => x.GetFiles(ScriptPath, SqlHelpers.SqlFileExtension)).Returns(scriptsFromFolder);
            _scriptsExecutor.ExecuteScripts(_databaseMock.Object);

            foreach (var script in scriptsFromFolder)
            {
                _sqlServerDataProviderMock.Verify(x => x.ExecuteScript(script.Content, DatabaseName), Times.Once);
                _loggerMock.Verify(x =>
                    x.LogEvent(SPublisherEvent.SqlScriptExecuted, It.Is<ISqlScriptInfo>(y => y.Path == script.Path)), Times.Once);
            }
        }

        [Fact]
        public void ShouldNotRunExcludedScripts()
        {
            var scriptsFromFolder = new IFile[]
            {
                new FileModel{ Path="firstScriptPath", Content = "firstScriptText", Hash = "firstScriptHash" },
                new FileModel{ Path="secondScriptPath", Content = "secondScriptText", Hash = "secondScriptHash"}
            };

            var excludedScripts = new[]
            {
                scriptsFromFolder[0]
            };

            _sqlServerDataProviderMock.Setup(x => x.GetHashInfoList(DatabaseName)).Returns(excludedScripts);
            _databaseMock.SetupGet(x => x.HashingEnabled).Returns(true);
            _databaseMock.SetupGet(x => x.DatabaseName).Returns(DatabaseName);
            _scriptsMock.SetupGet(x => x.IsFolder).Returns(true);
            _storageAccessorMock.Setup(x => x.GetFiles(ScriptPath, SqlHelpers.SqlFileExtension)).Returns(scriptsFromFolder);
             _scriptsExecutor.ExecuteScripts(_databaseMock.Object);

            _sqlServerDataProviderMock.Verify(x => x.ExecuteScript(scriptsFromFolder[0].Content, DatabaseName), Times.Never);
            _sqlServerDataProviderMock.Verify(x => x.ExecuteScript(scriptsFromFolder[1].Content, DatabaseName), Times.Once);
        }

        [Theory]
        [InlineData(true, "", 0)]
        [InlineData(true, DatabaseName, 1)]
        [InlineData(false, "", 0)]
        [InlineData(false, DatabaseName, 0)]
        public void ShouldCreateHashInfoTableIfHashingEnabled(bool hashingEnabled, string databaseName, int times)
        {
            _databaseMock.SetupGet(x => x.HashingEnabled).Returns(hashingEnabled);
            _databaseMock.SetupGet(x => x.DatabaseName).Returns(databaseName);
            _scriptsExecutor.ExecuteScripts(_databaseMock.Object);
            _sqlServerDataProviderMock.Verify(x => x.CreateHashInfoTableIfNotExists(databaseName), Times.Exactly(times));
        }

        [Theory]
        [InlineData(true, "", 0)]
        [InlineData(true, DatabaseName, 1)]
        [InlineData(false, "", 0)]
        [InlineData(false, DatabaseName, 0)]
        public void ShouldSaveHashInfoToDatabaseIfHashingEnabled(bool hashingEnabled, string databaseName, int times)
        {
            _databaseMock.SetupGet(x => x.HashingEnabled).Returns(hashingEnabled);
            _databaseMock.SetupGet(x => x.DatabaseName).Returns(databaseName);
            _scriptsExecutor.ExecuteScripts(_databaseMock.Object);
            _sqlServerDataProviderMock.Verify(x => x.SaveHashInfo(DatabaseName, File), Times.Exactly(times));
        }
    }
}