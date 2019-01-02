using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using FluentAssertions;
using Moq;
using MySql.Data.MySqlClient;
using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.Exceptions;
using SPublisher.DBManagement;
using Xunit;

namespace SPublisher.UnitTests.DBManagement
{
    public class DatabaseActionsExecutorTests
    {
        private const string FirstDbName = "FirstDbName";
        private const string SecondDbName = "SecondDbName";
        private readonly IDatabase _firstDatabase = new DatabaseModel
        {
            DatabaseName = FirstDbName,
            Scripts = new[]
        {
            new ScriptsModel()
        }
        };
        private readonly IDatabase _secondDatabase = new DatabaseModel { DatabaseName = SecondDbName };
        private readonly Mock<IDatabaseCreator> _databaseCreatorMock = new Mock<IDatabaseCreator>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<IScriptsExecutor> _scriptsExecutorMock = new Mock<IScriptsExecutor>();
        private readonly IDatabaseActionsExecutor _databaseActionsExecutor;

        public DatabaseActionsExecutorTests()
        {
            _databaseActionsExecutor = new DatabaseActionsExecutor(_databaseCreatorMock.Object, _scriptsExecutorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ShouldCreateDatabaseIfNotExist()
        {
            _databaseCreatorMock.Setup(x => x.Create(_firstDatabase)).Returns(DatabaseCreateResult.Success);
            _databaseCreatorMock.Setup(x => x.Create(_secondDatabase)).Returns(DatabaseCreateResult.AlreadyExists);

            _databaseActionsExecutor.Execute(_firstDatabase);
            _databaseActionsExecutor.Execute(_secondDatabase);

            _databaseCreatorMock.Verify(x => x.Create(_firstDatabase), Times.Once);
            _databaseCreatorMock.Verify(x => x.Create(_secondDatabase), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreationStarted, null), Times.Exactly(2));
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreated, _firstDatabase), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseExists, _secondDatabase), Times.Once);
        }

        [Fact]
        public void ShouldExecuteScriptsIfThereAreAny()
        {
            _databaseActionsExecutor.Execute(_firstDatabase);
            _scriptsExecutorMock.Verify(x => x.ExecuteScripts(_firstDatabase), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ScriptsExecutionStarted, _firstDatabase), Times.Once);
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.ScriptsExecutionCompleted, _firstDatabase), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateDatabaseIfNullOrEmpty()
        {

            _databaseActionsExecutor.Execute(new DatabaseModel());
            _loggerMock.Verify(x => x.LogEvent(SPublisherEvent.DatabaseCreationStarted, null), Times.Never);
        }

        [Fact]
        public void ShouldThrowDatabaseExceptionOnSqlException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(SqlException))
                as SqlException;
            _databaseCreatorMock.Setup(x => x.Create(_firstDatabase)).Throws(exception);
           
            Action action = () => { _databaseActionsExecutor.Execute(_firstDatabase); };
            action.Should().Throw<DatabaseException>();
        }

        [Fact]
        public void ShouldThrowDatabaseExceptionOnMySqlException()
        {
            var exception = FormatterServices.GetUninitializedObject(typeof(MySqlException))
                as MySqlException;
            _databaseCreatorMock.Setup(x => x.Create(_firstDatabase)).Throws(exception);

            Action action = () => { _databaseActionsExecutor.Execute(_firstDatabase); };
            action.Should().Throw<DatabaseException>();
        }

        [Fact]
        public void ShouldNotHandleOtherExceptions()
        {
            var exception = new Exception();
            _databaseCreatorMock.Setup(x => x.Create(_firstDatabase)).Throws(exception);

            Action action = () => { _databaseActionsExecutor.Execute(_firstDatabase); };
            action.Should().Throw<Exception>();
        }
    }
}