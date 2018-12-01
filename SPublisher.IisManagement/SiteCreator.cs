using System.Linq;
using SPublisher.Core;

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

        public void Create(IApplication[] applications)
        {
            if (applications == null || !applications.Any())
            {
                _logger.LogEvent(SPublisherEvent.ApplicationListIsEmpty);
                return;
            }

            using (_serverManagerCreator.ServerManager())
            {
                foreach (var application in applications)
                {
                    _applicationCreator.Create(application);
                }

                _serverManagerCreator.CommitChanges();
            }
        }
    }
}