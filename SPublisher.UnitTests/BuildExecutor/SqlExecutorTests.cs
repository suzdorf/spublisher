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
        private readonly IDatabaseCreate _firstDatabaseCreate = new DatabaseCreateModel {DbName = FirstDbName};
        private readonly IDatabaseCreate _secondDatabaseCreate = new DatabaseCreateModel { DbName = SecondDbName };
        private readonly Mock<IConnectionSetter> _connectionSetter = new Mock<IConnectionSetter>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly IBuildStepExecutor _buildStepExecutor;

        public SqlExecutorTests()
        {
            _buildStepExecutor = new SqlExecutor(_databaseCreatorMock.Object, _loggerMock.Object, _connectionSetter.Object);
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
            buildStep.As<ISqlStep>().SetupGet(x => x.DatabaseCreate).Returns(new[]
            {
                _firstDatabaseCreate,
                _secondDatabaseCreate
            });

            _databaseCreatorMock.Setup(x => x.Create(_firstDatabaseCreate)).Returns(DatabaseCreateResult.Success);
            _databaseCreatorMock.Setup(x => x.Create(_secondDatabaseCreate)).Returns(DatabaseCreateResult.AlreadyExists);

            _buildStepExecutor.Execute(buildStep.Object);

            _databaseCreatorMock.Verify(x=>x.Create(_firstDatabaseCreate), Times.Once);
            _databaseCreatorMock.Verify(x => x.Create(_secondDatabaseCreate), Times.Once);
            _loggerMock.Verify(x=>x.LogEvent(SPublisherEvent.DatabaseCreationStarted, null), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreationCompleted, null), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreated, _firstDatabaseCreate), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseExists, _secondDatabaseCreate), Times.Once);
        }
    }
}