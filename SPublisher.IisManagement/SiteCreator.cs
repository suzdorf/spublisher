using System.Linq;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using SPublisher.Core.Log;

namespace SPublisher.IisManagement
{
    public class SiteCreator : ISiteCreator
    {
        private readonly IServerManagerDataProvider _serverManagerDataProvider;
        private readonly IApplicationCreator _applicationCreator;
        private readonly IAppPoolCreator _appPoolCreator;
        private readonly IServerManagerAccessor _serverManagerAccessor;
        private readonly IBindingsManager _bindingsManager;
        private readonly ILogger _logger;

        public SiteCreator(IServerManagerAccessor serverManagerAccessor, IApplicationCreator applicationCreator, ILogger logger, IServerManagerDataProvider serverManagerDataProvider, IAppPoolCreator appPoolCreator, IBindingsManager bindingsManager)
        {
            _serverManagerAccessor = serverManagerAccessor;
            _applicationCreator = applicationCreator;
            _logger = logger;
            _serverManagerDataProvider = serverManagerDataProvider;
            _appPoolCreator = appPoolCreator;
            _bindingsManager = bindingsManager;
        }

        public void Create(ISite[] sites)
        {
            if (!sites.Any())
            {
                _logger.LogEvent(SPublisherEvent.ApplicationListIsEmpty);
                return;
            }

            using (_serverManagerAccessor.ServerManager())
            {
                foreach (var site in sites)
                {
                   CreateSite(site);
                }

                _serverManagerAccessor.CommitChanges();
            }
        }

        private void CreateSite(ISite site)
        {
            _appPoolCreator.Create(site);

            if (!_serverManagerDataProvider.SiteIsExist(site.Name))
            {
                var binding = site.Bindings.FirstOrDefault() ?? new DefaultBinding(site.Name);
                _serverManagerDataProvider.CreateSite(site, binding);
                _logger.LogEvent(SPublisherEvent.SiteCreated, site);
                _logger.LogEvent(SPublisherEvent.BindingAdded, binding);
            }
            else
            {
                _logger.LogEvent(SPublisherEvent.SiteExists, site);
            }

            if (site.Bindings.Any())
            {
                _bindingsManager.Manage(site.Name, site.Bindings);
            }

            if (site.Applications.Any())
                foreach (var app in site.Applications)
                {
                    _applicationCreator.Create(app, site.Name);
                }
        }
    }
}