
using SPublisher.Core.BuildSteps;

namespace SPublisher.Core
{
    public interface IConfiguration
    {
        IBuildStep[] BuildSteps { get; }

        IApplication[] Applications { get; }
    }
}
