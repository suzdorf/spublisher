using SPublisher.Core.Enums;

namespace SPublisher.Core.IisManagement
{
    public interface IBinding : ILogMessage
    {
        BindingType Type { get; }
        string IpAddress { get; }
        int Port { get; }
        string HostName { get; }
        string CertificateThumbPrint { get; }
    }
}