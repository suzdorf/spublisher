using System.Collections.Generic;
using Moq;
using SPublisher.Core;
using SPublisher.DBManagement;
using Xunit;

namespace SPublisher.UnitTests.DBManagement
{
    public class ScriptsExecutorTests
    {
        private const string DatabaseName = "DatabaseName";
        private const string ScriptPath = "ScriptPath";
        private const string Script = "Script";
        private readonly Mock<ISqlServerDataProvider> _sqlServerDataProviderMock = new Mock<ISqlServerDataProvider>();
        private readonly Mock<IStorageAccessor> _storageAccessorMock = new Mock<IStorageAccessor>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IDatabase> _databaseMock = new Mock<IDatabase>();
        private readonly Mock<IScripts> _scriptsMock = new Mock<IScripts>();
        private readonly IScriptsExecutor _scriptsExecutor;

        public ScriptsExecutorTests()
        {
            _scriptsMock.SetupGet(x => x.Path).Returns(ScriptPath);
            _storageAccessorMock.Setup(x => x.ReadAllText(ScriptPath)).Returns(Script);
            _databaseMock.SetupGet(x => x.Scripts).Returns(new[] {_scriptsMock.Object});
            _scriptsExecutor = new ScriptsExecutor(_sqlServerDataProviderMock.Object, _storageAccessorMock.Object, _loggerMock.Object);
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
        public void ShouldExecuteScriptUsingMasterIfDatabaseNameIsNull()
        {
            _scriptsExecutor.ExecuteScripts(_databaseMock.Object);
            _sqlServerDataProviderMock.Verify(x => x.ExecuteScript(Script, SqlHelpers.MasterDatabaseName), Times.Once);
            _loggerMock.Verify(x =>
                x.LogEvent(SPublisherEvent.SqlScriptExecuted, It.Is<ISqlScriptInfo>(y => y.Path == ScriptPath)), Times.Once);
        }

        [Fact]
        public void ShouldExecuteFilesInFolder()
        {
            var scriptsFromFolder = new Dictionary<string, string>
            {
                {"firstScriptPath", "firstScriptText" },
                {"secondScriptPath", "secondScriptText" }
            };
            _scriptsMock.SetupGet(x => x.IsFolder).Returns(true);
            _storageAccessorMock.Setup(x => x.ReadAllText(ScriptPath, SqlHelpers.SqlFileExtension)).Returns(scriptsFromFolder);
            _scriptsExecutor.ExecuteScripts(_databaseMock.Object);

            foreach (var keyValuePair in scriptsFromFolder)
            {
                _sqlServerDataProviderMock.Verify(x => x.ExecuteScript(keyValuePair.Value, SqlHelpers.MasterDatabaseName), Times.Once);
                _loggerMock.Verify(x =>
                    x.LogEvent(SPublisherEvent.SqlScriptExecuted, It.Is<ISqlScriptInfo>(y => y.Path == keyValuePair.Key)), Times.Once);
            }
        }
    }
}