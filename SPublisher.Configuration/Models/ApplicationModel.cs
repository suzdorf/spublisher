using SPublisher.Core.IisManagement;

// ReSharper disable CoVariantArrayConversion

namespace SPublisher.Configuration.Models
{
    public class ApplicationModel : IApplication
    {
        public string Name { get; set; }
        public string AppPoolName { get; set; }
        public string ManagedRuntimeVersion { get; set; }
        public string Path { get; set; }
        public ApplicationModel[] Applications { get; set; } = new ApplicationModel[0];
        public bool IsVirtualDirectory { get; set; }
        IApplication[] IApplication.Applications => Applications;
    }
}