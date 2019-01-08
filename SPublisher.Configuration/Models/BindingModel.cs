using Newtonsoft.Json;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;

namespace SPublisher.Configuration.Models
{
    public class BindingModel : IBinding
    {
        private int? _port;
        public string IpAddress { get; set; } = Constants.SiteBinding.DefaultIpAddress;

        public int Port
        {
            get => _port ?? (IsHttp ? Constants.SiteBinding.DefaultHttpPort : Constants.SiteBinding.DefaultHttpsPort);
            set => _port = value;
        }

        public string HostName { get; set; } = string.Empty;
        public string CertificateThumbPrint { get; set; }

        [JsonIgnore]
        BindingType IBinding.Type => IsHttp ? BindingType.Http : BindingType.Https;

        [JsonIgnore]
        private bool IsHttp => string.IsNullOrEmpty(CertificateThumbPrint);
    }
}