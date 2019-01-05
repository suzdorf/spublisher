using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public interface IAppPoolCreator
    {
        void Create(IAppPoolInfo info);
    }
}