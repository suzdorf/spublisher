using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.Exceptions;

namespace SPublisher.Configuration.Exceptions
{
    public class BuildStepTypeNotFoundException : SPublisherException, IBuildStepTypeNotFoundMessage
    {
        public BuildStepTypeNotFoundException(string type)
        {
            Type = type;
        }

        public string Type { get; }
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.BuildStepTypeNotFound;
    }
}