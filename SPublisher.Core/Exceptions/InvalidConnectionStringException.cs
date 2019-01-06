using SPublisher.Core.Enums;

namespace SPublisher.Core.Exceptions
{
    public class InvalidConnectionStringException :SPublisherException
    {
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.InvalidConnectionStringFormat;
    }
}