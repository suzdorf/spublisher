using System.IO;
using System.Linq;
using SPublisher.Core;

namespace SPublisher.IisManagement
{
    public class ServerManagerDataProvider : IServerManagerDataProvider
    {
        private readonly IServerManagerStorage _storage;
        public ServerManagerDataProvider(IServerManagerStorage storage)
        {
            _storage = storage;
        }

        public bool PoolIsExist(string appPoolName)
        {
            return _storage.Get().ApplicationPools[appPoolName] != null;
        }

        public bool SiteIsExist(string siteName)
        {
            return _storage.Get().Sites[siteName] != null;
        }

        public bool ApplicationIsExist(string siteName, string path)
        {
            var parentSite = _storage.Get().Sites[siteName];
            return parentSite?.Applications.FirstOrDefault(x => x.Path == path) != null;
        }

        public void CreateAppPool(IAppPoolInfo info)
        {
            var iisAppPool = _storage.Get().ApplicationPools.Add(info.AppPoolName);
            iisAppPool.ManagedRuntimeVersion = info.ManagedRuntimeVersion;
        }

        public void CreateApplication(IApplicationInfo info, string siteName, string path)
        {
            var app = _storage.Get().Sites[siteName].Applications.Add($"{path}{info.Name}", Path.GetFullPath(info.Path));

            if (!string.IsNullOrEmpty(info.AppPoolName))
            {
                app.ApplicationPoolName = info.AppPoolName;
            }
        }

        public void CreateSite(IApplicationInfo info)
        {
            var iisSite = _storage.Get().Sites.Add(info.Name, "http", $"*:80:{info.Name}",
                Path.GetFullPath(info.Path));
            iisSite.ApplicationDefaults.ApplicationPoolName = info.AppPoolName;
        }

        public bool VirtualDirectoryIsExist(string siteName, string path)
        {
            var parentSite = _storage.Get().Sites[siteName];
            return parentSite?.Applications[0].VirtualDirectories.FirstOrDefault(x => x.Path == path) != null;
        }

        public void CreateVirtualDirectory(IApplicationInfo info, string siteName, string path)
        {
            _storage.Get().Sites[siteName].Applications[0].VirtualDirectories.Add($"{path}{info.Name}", Path.GetFullPath(info.Path));
        }
    }
}
