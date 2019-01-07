using System.Linq;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.DbManagement;

namespace SPublisher.BuildExecutor.BuildStepExecutors
{
    public class SqlExecutor : IBuildStepExecutor
    {
        private readonly IConnectionSetter _connectionSetter;
        private readonly IDatabaseActionsExecutor _databaseActionsExecutor;

        public SqlExecutor(IConnectionSetter connectionSetter, IDatabaseActionsExecutor databaseActionsExecutor)
        {
            _connectionSetter = connectionSetter;
            _databaseActionsExecutor = databaseActionsExecutor;
        }

        public ExecutionResult Execute(IBuildStep buildStep)
        {
            var step = (ISqlStep)buildStep;
            _connectionSetter.Set(step);

            if (step.Databases != null && step.Databases.Any())
            {
                foreach (var database in step.Databases)
                {
                    _databaseActionsExecutor.Execute(database);
                }
            }

            return ExecutionResult.Success;
        }
    }
}