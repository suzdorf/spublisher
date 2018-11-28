using System;
using Microsoft.Web.Administration;

namespace SPublisher.IisManagement
{
    public class ServerManagerAccessor : IServerManagerAccessor, IServerManagerStorage
    {
        private ServerManager _manager;
        public IDisposable ServerManager()
        {
            _manager = new ServerManager();
            return _manager;
        }

        public void CommitChanges()
        {
           _manager.CommitChanges();
        }

        public ServerManager Get()
        {
            return _manager;
        }
    }
}