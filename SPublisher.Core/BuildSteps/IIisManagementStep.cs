using SPublisher.Core.IisManagement;

namespace SPublisher.Core.BuildSteps
{
    public interface IIisManagementStep : IBuildStep
    {
        ISite[] Sites { get; }
    }
}