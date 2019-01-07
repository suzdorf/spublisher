using SPublisher.Core.Log;

namespace SPublisher.Core.ExceptionMessages
{
    public interface ICertificateNotFoundMessage : ILogMessage
    {
        string CertificateThumbprint { get; }
    }
}