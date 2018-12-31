using System;

namespace SPublisher.Core
{
    public interface IStorageLogger
    {
        void LogError(Exception exception);
    }
}