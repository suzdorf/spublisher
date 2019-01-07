using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;
using SPublisher.Core.Log;

namespace SPublisher.IisManagement
{
    public class AppPoolCreator : IAppPoolCreator
    {
        private readonly IServerManagerDataProvider _serverManagerDataProvider;
        private readonly ILogger _logger;

        public AppPoolCreator(IServerManagerDataProvider serverManagerDataProvider, ILogger logger)
        {
            _serverManagerDataProvider = serverManagerDataProvider;
            _logger = logger;
        }

        public void Create(IAppPoolInfo info)
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