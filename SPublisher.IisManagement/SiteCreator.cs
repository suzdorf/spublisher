using SPublisher.Core;

namespace SPublisher.IisManagement
{
    public class SiteCreator : ISiteCreator
    {
        private readonly IApplicationCreator _applicationCreator;
        private readonly IServerManagerAccessor _serverManagerCreator;

        public SiteCreator(IServerManagerAccessor serverManagerCreator, IApplicationCreator applicationCreator)
        {
            _serverManagerCreator = serverManagerCreator;
            _applicationCreator = applicationCreator;
        }

        public void Create(IApplication[] applications)
        {
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