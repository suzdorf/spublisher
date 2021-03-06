﻿using SPublisher.Core.Enums;
using SPublisher.Core.Exceptions;

namespace SPublisher.Configuration.Exceptions
{
    public class InvalidJsonException : SPublisherException
    {
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.InvalidJson;
    }
}