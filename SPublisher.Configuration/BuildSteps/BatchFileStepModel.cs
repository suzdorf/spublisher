using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class BatchFileStepModel : BuildStepModel, IBatchFileStep
    {
        public string FileName { get; set; }
    }
}
