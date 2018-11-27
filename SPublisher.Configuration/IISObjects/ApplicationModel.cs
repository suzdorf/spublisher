using SPublisher.Core;
// ReSharper disable CoVariantArrayConversion

namespace SPublisher.Configuration.IISObjects
{
    public class ApplicationModel : IApplication
    {
        public string Name { get; set; }
        public string AppPoolName { get; set; }
        public string ManagedRuntimeVersion { get; set; }
        public string Path { get; set; }
        public ApplicationModel[] Applications { get; set; }

        IApplication[] IApplication.Applications
        {
            get { return Applications; }
        }
    }
}