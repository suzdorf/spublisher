using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor.BuildStepExecutors
{
    public class SqlExecutor : IBuildStepExecutor
    {
        private readonly IDatabaseCreator _databaseCreator;
        private readonly IConnectionSetter _connectionSetter;
        private readonly ILogger _logger;

        public SqlExecutor(IDatabaseCreator databaseCreator, ILogger logger, IConnectionSetter connectionSetter)
        {
            _databaseCreator = databaseCreator;
            _logger = logger;
            _connectionSetter = connectionSetter;
        }

        public ExecutionResult Execute(IBuildStep buildStep)
        {
            var step = (ISqlStep) buildStep;
            _connectionSetter.SetConnectionString(step.ConnectionString);

            if (step.DatabaseCreate != null && step.DatabaseCreate.Any())
            {
                _logger.LogEvent(SPublisherEvent.DatabaseCreationStarted);
                foreach (var databaseCreate in step.DatabaseCreate)
                {
                    var result = _databaseCreator.Create(databaseCreate);

                    if (result == DatabaseCreateResult.AlreadyExists)
                    {
                        _logger.LogEvent(SPublisherEvent.DatabaseExists, databaseCreate);
                    }

                    if (result == DatabaseCreateResult.Success)
                    {
                        _logger.LogEvent(SPublisherEvent.DatabaseCreated, databaseCreate);
                    }
                }
                _logger.LogEvent(SPublisherEvent.DatabaseCreationCompleted);
            }

            return ExecutionResult.Success;
        }
    }
}