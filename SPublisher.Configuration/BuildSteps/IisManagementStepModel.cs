using SPublisher.Configuration.IISObjects;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class IisManagementStepModel : BuildStepModel, IIisManagementStep
    {
        public ApplicationModel[] Applications { get; set; }

        IApplication[] IIisManagementStep.Applications
        {
            get { return Applications; }
        }
    }
}