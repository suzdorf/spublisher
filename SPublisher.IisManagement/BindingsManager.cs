using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.IisManagement;

namespace SPublisher.IisManagement
{
    public class BindingsManager : IBindingsManager
    {
        private readonly IServerManagerDataProvider _serverManagerDataProvider;
        private readonly ILogger _logger;

        public BindingsManager(IServerManagerDataProvider serverManagerDataProvider, ILogger logger)
        {
            _serverManagerDataProvider = serverManagerDataProvider;
            _logger = logger;
        }

        public void Manage(string siteName, IBinding[] bindings)
        {
            foreach (var binding in bindings)
            {
                if (!_serverManagerDataProvider.BindingExists(binding, siteName))
                {
                    _serverManagerDataProvider.AddBinding(binding, siteName);
                    _logger.LogEvent(SPublisherEvent.BindingAdded, binding);
                }
            }
        }
    }
}