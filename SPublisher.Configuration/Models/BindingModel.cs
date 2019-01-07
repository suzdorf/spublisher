using Newtonsoft.Json;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;

namespace SPublisher.Configuration.Models
{
    public class BindingModel : IBinding
    {
        public string IpAddress { get; set; } = Constants.SiteBinding.DefaultIpAddress;
        public int Port { get; set; } = Constants.SiteBinding.DefaultPort;
        public string HostName { get; set; } = string.Empty;
        public string CertificateThumbPrint { get; set; }

        [JsonIgnore]
        BindingType IBinding.Type => string.IsNullOrEmpty(CertificateThumbPrint) ? BindingType.Http : BindingType.Https;
    }
}