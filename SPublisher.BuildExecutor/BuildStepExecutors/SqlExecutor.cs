using System.Data.SqlClient;
using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Exceptions;

namespace SPublisher.BuildExecutor.BuildStepExecutors
{
    public class SqlExecutor : IBuildStepExecutor
    {
        private readonly IDatabaseCreator _databaseCreator;
        private readonly IScriptsExecutor _scriptsExecutor;
        private readonly IConnectionSetter _connectionSetter;
        private readonly ILogger _logger;

        public SqlExecutor(
            IDatabaseCreator databaseCreator,
            ILogger logger,
            IConnectionSetter connectionSetter,
            IScriptsExecutor scriptsExecutor)
        {
            _databaseCreator = databaseCreator;
            _logger = logger;
            _connectionSetter = connectionSetter;
            _scriptsExecutor = scriptsExecutor;
        }

        public ExecutionResult Execute(IBuildStep buildStep)
        {
            try
            {
                var step = (ISqlStep)buildStep;
                _connectionSetter.SetConnectionString(step.ConnectionString);

                if (step.Databases != null && step.Databases.Any())
                {
                    foreach (var database in step.Databases)
                    {
                        if (!string.IsNullOrEmpty(database.DatabaseName))
                        {
                            _logger.LogEvent(SPublisherEvent.DatabaseCreationStarted);

                            var result = _databaseCreator.Create(database);

                            if (result == DatabaseCreateResult.AlreadyExists)
                            {
                                _logger.LogEvent(SPublisherEvent.DatabaseExists, database);
                            }

                            if (result == DatabaseCreateResult.Success)
                            {
                                _logger.LogEvent(SPublisherEvent.DatabaseCreated, database);
                            }
                        }

                        if (database.Scripts.Any())
                        {
                            _logger.LogEvent(SPublisherEvent.ScriptsExecutionStarted, database);
                            _scriptsExecutor.ExecuteScripts(database);
                            _logger.LogEvent(SPublisherEvent.ScriptsExecutionCompleted, database);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new DatabaseException(e.Message);
            }

            return ExecutionResult.Success;
        }
    }
}