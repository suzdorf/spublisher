using SPublisher.Core;

namespace SPublisher.IisManagement
{
    public class ApplicationCreator : IApplicationCreator
    {
        private readonly IServerManagerDataProvider _serverManagerDataProvider;

        public ApplicationCreator(IServerManagerDataProvider serverManagerDataProvider)
        {
            _serverManagerDataProvider = serverManagerDataProvider;
        }

        public void Create(IApplication application)
        {
            if (!_serverManagerDataProvider.PoolIsExist((application as IAppPoolInfo).AppPoolName))
            {
                _serverManagerDataProvider.CreateAppPool(application);
            }

            if (!_serverManagerDataProvider.SiteIsExist(application.Name))
            {
                _serverManagerDataProvider.CreateSite(application);
            }

            foreach (var app in application.Applications)
            {
              CreateApplication(app, application.Name, "/");
            }
        }

        private void CreateApplication(IApplication application, string siteName, string path)
        {

            if (!_serverManagerDataProvider.PoolIsExist(((IAppPoolInfo)application).AppPoolName))
            {
                _serverManagerDataProvider.CreateAppPool(application);
            }

            if (!_serverManagerDataProvider.ApplicationIsExist(siteName, $"{path}{application.Name}"))
            {
                _serverManagerDataProvider.CreateApplication(application, siteName, path);
            }

            foreach (var app in application.Applications)
            {
                CreateApplication(app, siteName, $"{path}{application.Name}/");
            }
        }
    }
}