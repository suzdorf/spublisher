using System.Linq;
using SPublisher.Core;

namespace SPublisher.IisManagement
{
    public class ApplicationCreator : IApplicationCreator
    {
        private readonly IServerManagerDataProvider _serverManagerDataProvider;
        private readonly ILogger _logger;

        public ApplicationCreator(IServerManagerDataProvider serverManagerDataProvider, ILogger logger)
        {
            _serverManagerDataProvider = serverManagerDataProvider;
            _logger = logger;
        }

        public void Create(IApplication application)
        {
            CreateAppPool(application);

            if (!_serverManagerDataProvider.SiteIsExist(application.Name))
            {
                _serverManagerDataProvider.CreateSite(application);
                _logger.LogEvent(SPublisherEvent.SiteCreated, application);
            }
            else
            {
                _logger.LogEvent(SPublisherEvent.SiteExists, application);
            }

            if (application.Applications != null && application.Applications.Any())
                foreach (var app in application.Applications)
                {
                  CreateApplication(app, application.Name, "/");
                }
        }

        private void CreateApplication(IApplication application, string siteName, string path)
        {
            if (!application.IsVirtualDirectory)
            {
                CreateAppPool(application);

                if (!_serverManagerDataProvider.ApplicationIsExist(siteName, $"{path}{application.Name}"))
                {
                    _serverManagerDataProvider.CreateApplication(application, siteName, path);
                    _logger.LogEvent(SPublisherEvent.ApplicationCreated, application);
                }
                else
                {
                    _logger.LogEvent(SPublisherEvent.ApplicationExists, application);
                }
            }
            else
            {
                if (!_serverManagerDataProvider.VirtualDirectoryIsExist(application.Name, siteName, path))
                {
                    _serverManagerDataProvider.CreateVirtualDirectory(application, siteName, path);
                    _logger.LogEvent(SPublisherEvent.VirtualDirectoryCreated, application);
                }
                else
                {
                    _logger.LogEvent(SPublisherEvent.VirtualDirectoryExists, application);
                }
            }

            if (application.Applications != null && application.Applications.Any())
                foreach (var app in application.Applications)
                {
                    CreateApplication(app, siteName, $"{path}{application.Name}/");
                }
        }

        private void CreateAppPool(IAppPoolInfo info)
        {
            if (string.IsNullOrEmpty(info.AppPoolName)) return;

            if (!_serverManagerDataProvider.PoolIsExist(info.AppPoolName))
            {
                _serverManagerDataProvider.CreateAppPool(info);
                _logger.LogEvent(SPublisherEvent.ApplicationPoolCreated, info);
            }
            else
            {
                _logger.LogEvent(SPublisherEvent.ApplicationPoolExists, info);
            }
        }
    }
}