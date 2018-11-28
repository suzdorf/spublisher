using System;

namespace SPublisher.IisManagement
{
    public interface IServerManagerAccessor
    {
        IDisposable ServerManager();

        void CommitChanges();
    }
}