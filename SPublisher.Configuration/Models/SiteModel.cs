using SPublisher.Core.IisManagement;

namespace SPublisher.Configuration.Models
{
    public class SiteModel : ApplicationModel, ISite
    {
        public BindingModel[] Bindings { get; set; } = new BindingModel[0];
        IBinding[] ISite.Bindings => Bindings;
    }
}