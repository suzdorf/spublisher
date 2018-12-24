using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class SqlStepModel : BuildStepModel, ISqlStep
    {
        public string ConnectionString { get; set; }

        public DatabaseModel[] Databases { get; set; }
        IDatabase[] ISqlStep.Databases => Databases;
    }
}