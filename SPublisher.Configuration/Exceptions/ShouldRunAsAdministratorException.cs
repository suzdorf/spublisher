﻿using SPublisher.Core.Enums;
using SPublisher.Core.Exceptions;

namespace SPublisher.Configuration.Exceptions
{
    public class ShouldRunAsAdministratorException : SPublisherException
    {
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.ShouldRunAsAdministrator;
    }
}