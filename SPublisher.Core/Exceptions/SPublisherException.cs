using System;

namespace SPublisher.Core.Exceptions
{
    public abstract class SPublisherException : Exception
    {
        public abstract SPublisherEvent SPublisherEvent { get; }
    }
}