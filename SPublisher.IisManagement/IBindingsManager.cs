using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public interface IBindingsManager
    {
        void Manage(string siteName, IBinding[] bindings);
    }
}