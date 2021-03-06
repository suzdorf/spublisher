﻿using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Administration;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.Exceptions;
using SPublisher.Core.IisManagement;

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
        
        public void CreateSite(IApplicationInfo info, IBinding binding)
        {
            Site iisSite;
            if (binding.Type == BindingType.Https)
            {
                iisSite = _storage.Get().Sites.Add(
                    info.Name,
                    GetBindingInformation(binding),
                    Path.GetFullPath(info.Path),
                    GetCertificateHash(binding.CertificateThumbPrint),
                    Constants.SiteBinding.CertificateStoreName);
            }
            else
            {
                iisSite = _storage.Get().Sites.Add(
                    info.Name,
                    GetBindingProtocol(binding.Type),
                    GetBindingInformation(binding),
                    Path.GetFullPath(info.Path));
            }


            if (!string.IsNullOrEmpty(info.AppPoolName))
            {
                iisSite.ApplicationDefaults.ApplicationPoolName = info.AppPoolName;
            }
        }

        public bool VirtualDirectoryIsExist(string directoryName, string siteName, string path)
        {
            return GetApplication(siteName, path)?.VirtualDirectories.FirstOrDefault(x => x.Path == $"/{directoryName}") != null;
        }

        public void CreateVirtualDirectory(IApplicationInfo info, string siteName, string path)
        {
            GetApplication(siteName, path)?.VirtualDirectories.Add($"{GetVirtualDirectoryPath(path)}{info.Name}", Path.GetFullPath(info.Path));
        }

        public bool BindingExists(IBinding binding, string siteName)
        {
            return _storage.Get().Sites[siteName].Bindings.FirstOrDefault(x =>
                       x.Protocol == GetBindingProtocol(binding.Type) &&
                       x.BindingInformation == GetBindingInformation(binding)) != null;
        }

        public void AddBinding(IBinding binding, string siteName)
        {
            var bindings = _storage.Get().Sites[siteName].Bindings;
            if (binding.Type == BindingType.Https)
            {
                bindings.Add(
                    GetBindingInformation(binding),
                    GetCertificateHash(binding.CertificateThumbPrint),
                    Constants.SiteBinding.CertificateStoreName);
            }
            else
            {
                bindings.Add(
                    GetBindingInformation(binding),
                    GetBindingProtocol(binding.Type));
            }
        }

        private Application GetApplication(string siteName, string path)
        {
            var parentSite = _storage.Get().Sites[siteName];
            var appName = path.Split('/')[1];
            return string.IsNullOrEmpty(appName) ? parentSite?.Applications[0] : parentSite?.Applications[$"/{appName}"];
        }

        private string GetVirtualDirectoryPath(string path)
        {
            var appName = path.Split('/')[1];

            if (string.IsNullOrEmpty(appName))
            {
                return path;
            }

            return path.Substring(appName.Length + 1, path.Length - appName.Length - 1);
        }

        private static string GetBindingInformation(IBinding binding)
        {
            return $"{binding.IpAddress}:{binding.Port}:{binding.HostName}";
        }

        private static string GetBindingProtocol(BindingType type)
        {
            return Constants.SiteBinding.Types.BuildDictionary[type];
        }

        private static byte[] GetCertificateHash(string certificateThumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly);
            var certificate =
                store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint.ToUpper(), false);

            if (certificate.Count < 1)
            {
                throw new CertificateNotFoundException(certificateThumbprint);
            }

            return certificate[0].GetCertHash();
        }
    }
}
