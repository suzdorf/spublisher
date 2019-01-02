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
        
        private readonly Mock<IConnectionSetter> _connectionSetter = new Mock<IConnectionSetter>();
        private readonly Mock<IDatabaseActionsExecutor> _databaseActionsExecutorMock = new Mock<IDatabaseActionsExecutor>();
        private readonly IBuildStepExecutor _buildStepExecutor;

        public SqlExecutorTests()
        {
            _buildStepExecutor = new SqlExecutor(_connectionSetter.Object, _databaseActionsExecutorMock.Object);
        }

        [Fact]
        public void ShouldSetConnection()
        {
            var buildStep = new Mock<ISqlStep>();
            _buildStepExecutor.Execute(buildStep.Object);

            _connectionSetter.Verify(x=>x.Set(buildStep.Object), Times.Once);
        }

        [Fact]
        public void ShouldCallExecuteForEachDatabase()
        {
            var databases = new[]
            {
                new DatabaseModel(),
                new DatabaseModel()
            };
            var buildStep = new Mock<ISqlStep>();
            buildStep.SetupGet(x => x.Databases).Returns(databases);
            _buildStepExecutor.Execute(buildStep.Object);

            foreach (var databaseModel in databases)
            {
                _databaseActionsExecutorMock.Verify(x=>x.Execute(databaseModel), Times.Once);
            }
        }
    }
}