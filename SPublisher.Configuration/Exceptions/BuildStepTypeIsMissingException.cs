using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.Exceptions;

namespace SPublisher.Configuration.Exceptions
{
    public class BuildStepTypeIsMissingException : SPublisherException
    {
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.BuildStepTypeIsMissing;
    }
}