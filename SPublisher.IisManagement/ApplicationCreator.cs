using System.Linq;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using SPublisher.Core.Log;

namespace SPublisher.IisManagement
{
    public class ApplicationCreator : IApplicationCreator
    {
        private readonly IServerManagerDataProvider _serverManagerDataProvider;
        private readonly ILogger _logger;
        private readonly IAppPoolCreator _appPoolCreator;

        public ApplicationCreator(IServerManagerDataProvider serverManagerDataProvider, ILogger logger, IAppPoolCreator appPoolCreator)
        {
            _serverManagerDataProvider = serverManagerDataProvider;
            _logger = logger;
            _appPoolCreator = appPoolCreator;
        }

        public void Create(IApplication application, string siteName, string path = "/")
        {
            if (!application.IsVirtualDirectory)
            {
                _appPoolCreator.Create(application);

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
                    Create(app, siteName, $"{path}{application.Name}/");
                }
        }
    }
}