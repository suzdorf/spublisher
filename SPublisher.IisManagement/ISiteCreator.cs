using SPublisher.Core;

namespace SPublisher.IisManagement
{
    public interface ISiteCreator
    {
        void Create(IApplication[] applications);
    }
}