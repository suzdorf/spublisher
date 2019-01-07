using SPublisher.Core.Enums;
using SPublisher.Core.ExceptionMessages;

namespace SPublisher.Core.Exceptions
{
    public class CertificateNotFoundException : SPublisherException, ICertificateNotFoundMessage
    {
        public CertificateNotFoundException(string certificateThumbprint)
        {
            CertificateThumbprint = certificateThumbprint;
        }

        public override SPublisherEvent SPublisherEvent => SPublisherEvent.CertificateNotFound;

        public string CertificateThumbprint { get; }
    }
}