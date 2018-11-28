using System;
using System.Linq;
using SPublisher.Core;

namespace SPublisher.IisManagement
{
    public class DefaultSiteCreator : ISiteCreator
    {
        private readonly IApplicationCreator _applicationCreator;
        private readonly IServerManagerAccessor _serverManagerCreator;

        public DefaultSiteCreator(IServerManagerAccessor serverManagerCreator, IApplicationCreator applicationCreator)
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