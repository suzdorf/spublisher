﻿using SPublisher.Core;

namespace SPublisher.IisManagement
{
    public interface IServerManagerDataProvider
    {
        bool PoolIsExist(string appPoolName);
        bool SiteIsExist(string siteName);
        bool ApplicationIsExist(string siteName, string path);
        void CreateAppPool(IAppPoolInfo info);
        void CreateApplication(IApplicationInfo info, string siteName, string path);
        void CreateSite(IApplicationInfo info);
    }
}