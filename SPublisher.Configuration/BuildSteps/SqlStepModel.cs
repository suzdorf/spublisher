using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class SqlStepModel : BuildStepModel, ISqlStep
    {
        public string ConnectionString { get; set; }

        public DatabaseCreateModel[] DatabaseCreate { get; set; }
        IDatabaseCreate[] ISqlStep.DatabaseCreate => DatabaseCreate;
    }
}