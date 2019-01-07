using System;

namespace SPublisher.Core.Log
{
    public interface IStorageLogger
    {
        void LogError(Exception exception);
    }
}