using SPublisher.Configuration.Models;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.IisManagement;

namespace SPublisher.Configuration.BuildSteps
{
    public class IisManagementStepModel : BuildStepModel, IIisManagementStep
    {
        public SiteModel[] Sites { get; set; } = new SiteModel[0];

        ISite[] IIisManagementStep.Sites => Sites;
    }
}