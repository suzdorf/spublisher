using SPublisher.Core.IisManagement;

namespace SPublisher.Configuration.Models
{
    public class SiteModel : ApplicationModel, ISite
    {
        public BindingModel[] Bindings { get; set; }
        IBinding[] ISite.Bindings => Bindings;
    }
}