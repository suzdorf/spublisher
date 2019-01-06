using System.Linq;
using Newtonsoft.Json;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;

namespace SPublisher.Configuration.Models
{
    public class BindingModel : IBinding
    {
        public string Type { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string CertificateThumbPrint { get; set; }

        [JsonIgnore]
        BindingType IBinding.Type
        {
            get
            {
                var typeKeyValuePair = Constants.SiteBinding.Types.BuildDictionary.FirstOrDefault(x => x.Value == Type);

                if (string.IsNullOrEmpty(Type))
                {
                    return BindingType.Http;
                }

                if (typeKeyValuePair.Value != null)
                {
                    return typeKeyValuePair.Key;
                }

                return BindingType.Invalid;
            }
        }
    }
}