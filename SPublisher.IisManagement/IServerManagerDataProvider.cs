using SPublisher.Core;
using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public interface IServerManagerDataProvider
    {
        bool PoolIsExist(string appPoolName);
        bool SiteIsExist(string siteName);
        bool ApplicationIsExist(string siteName, string path);
        void CreateAppPool(IAppPoolInfo info);
        void CreateApplication(IApplicationInfo info, string siteName, string path);
        void CreateSite(IApplicationInfo info, IBinding binding);
        bool VirtualDirectoryIsExist(string directoryName, string siteName, string path);
        void CreateVirtualDirectory(IApplicationInfo info, string siteName, string path);
        bool BindingExists(IBinding binding, string siteName);
        void AddBinding(IBinding binding, string siteName);
    }
}