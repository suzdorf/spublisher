using System.Linq;
using SPublisher.Core;
using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public class SiteCreator : ISiteCreator
    {
        private readonly IApplicationCreator _applicationCreator;
        private readonly IServerManagerAccessor _serverManagerCreator;
        private readonly ILogger _logger;

        public SiteCreator(IServerManagerAccessor serverManagerCreator, IApplicationCreator applicationCreator, ILogger logger)
        {
            _serverManagerCreator = serverManagerCreator;
            _applicationCreator = applicationCreator;
            _logger = logger;
        }

        public void Create(ISite[] sites)
        {
            if (sites == null || !sites.Any())
            {
                _logger.LogEvent(SPublisherEvent.ApplicationListIsEmpty);
                return;
            }

            using (_serverManagerCreator.ServerManager())
            {
                foreach (var site in sites)
                {
                    _applicationCreator.Create(site);
                }

                _serverManagerCreator.CommitChanges();
            }
        }
    }
}