using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.Exceptions;

namespace SPublisher.BuildExecutor.Exceptions
{
    public class CommandLineStartException : SPublisherException
    {
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.CommandLineCouldNotStart;
    }
}