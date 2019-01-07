using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public interface IApplicationCreator
    {
        void Create(IApplication application, string siteName, string path = "/");
    }
}