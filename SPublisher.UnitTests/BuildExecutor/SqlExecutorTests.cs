using Moq;
using SPublisher.BuildExecutor;
using SPublisher.BuildExecutor.BuildStepExecutors;
using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using Xunit;

namespace SPublisher.UnitTests.BuildExecutor
{
    public class SqlExecutorTests
    {
        private const string FirstDbName = "FirstDbName";
        private const string SecondDbName = "SecondDbName";
        private const string ConnectionString = "ConnectionString";
        private readonly Mock<IDatabaseCreator> _databaseCreatorMock = new Mock<IDatabaseCreator>();
        private readonly IDatabase _firstDatabase = new DatabaseModel {DatabaseName = FirstDbName, Scripts = new []
        {
            new ScriptsModel()
        }};
        private readonly IDatabase _secondDatabase = new DatabaseModel { DatabaseName = SecondDbName };
        private readonly Mock<IConnectionSetter> _connectionSetter = new Mock<IConnectionSetter>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IScriptsExecutor> _scriptsExecutorMock = new Mock<IScriptsExecutor>();
        private readonly IBuildStepExecutor _buildStepExecutor;

        public SqlExecutorTests()
        {
            _buildStepExecutor = new SqlExecutor(_databaseCreatorMock.Object, _loggerMock.Object, _connectionSetter.Object, _scriptsExecutorMock.Object);
        }

        [Fact]
        public void ShouldSetConnection()
        {
            var buildStep = new Mock<IBuildStep>();
            buildStep.As<ISqlStep>().SetupGet(x => x.ConnectionString).Returns(ConnectionString);
            _buildStepExecutor.Execute(buildStep.Object);

            _connectionSetter.Verify(x=>x.SetConnectionString(ConnectionString), Times.Once);
        }

        [Fact]
        public void ShouldCreateDatabaseIfNotExist()
        {
            var buildStep = new Mock<IBuildStep>();
            buildStep.As<ISqlStep>().SetupGet(x => x.Databases).Returns(new[]
            {
                _firstDatabase,
                _secondDatabase
            });

            _databaseCreatorMock.Setup(x => x.Create(_firstDatabase)).Returns(DatabaseCreateResult.Success);
            _databaseCreatorMock.Setup(x => x.Create(_secondDatabase)).Returns(DatabaseCreateResult.AlreadyExists);

            _buildStepExecutor.Execute(buildStep.Object);

            _databaseCreatorMock.Verify(x => x.Create(_firstDatabase), Times.Once);
            _databaseCreatorMock.Verify(x => x.Create(_secondDatabase), Times.Once);
            _loggerMock.Verify(x=>x.LogEvent(SPublisherEvent.DatabaseCreationStarted, null), Times.Exactly(2));
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreationCompleted, null), Times.Exactly(2));
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreated, _firstDatabase), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseExists, _secondDatabase), Times.Once);
        }

        [Fact]
        public void ShouldExecuteScriptsIfThereAreAny()
        {
            var buildStep = new Mock<IBuildStep>();
            buildStep.As<ISqlStep>().SetupGet(x => x.Databases).Returns(new[]
            {
                _firstDatabase
            });

            _buildStepExecutor.Execute(buildStep.Object);
            _scriptsExecutorMock.Verify(x => x.ExecuteScripts(_firstDatabase), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ScriptsExecutionStarted, _firstDatabase), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ScriptsExecutionCompleted, _firstDatabase), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateDatabaseIfNullOrEmpty()
        {
            var buildStep = new Mock<IBuildStep>();
            buildStep.As<ISqlStep>().SetupGet(x => x.Databases).Returns(new IDatabase[]
            {
                new DatabaseModel()
            });

            _buildStepExecutor.Execute(buildStep.Object);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreationStarted, null), Times.Never);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreationCompleted, null), Times.Never);
        }
    }
}