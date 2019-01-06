using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public class DefaultBinding : IBinding
    {
        public DefaultBinding(string hostName)
        {
            HostName = hostName;
        }

        public BindingType Type => BindingType.Http;
        public string IpAddress => Constants.SiteBinding.DefaultIpAddress;
        public int Port => Constants.SiteBinding.DefaultPort;
        public string HostName { get; }
        public string CertificateThumbPrint => null;
    }
}