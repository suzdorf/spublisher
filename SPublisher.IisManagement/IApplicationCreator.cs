using SPublisher.Core;
using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public interface IApplicationCreator
    {
        void Create(IApplication application);
    }
}