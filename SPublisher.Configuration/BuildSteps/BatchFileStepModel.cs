using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    class BatchFileStepModel : BuildStepModel, IBatchFileStep
    {
        public string FileName { get; set; }
    }
}
