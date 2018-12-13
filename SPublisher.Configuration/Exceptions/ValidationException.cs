﻿using System.Collections.Generic;
using SPublisher.Core.Exceptions;

namespace SPublisher.Configuration.Exceptions
{
    public class ValidationException : SPublisherException
    {
        public readonly IReadOnlyList<IBuildStepValidationResult> ValidationResults;

        public ValidationException(IReadOnlyList<IBuildStepValidationResult> validationResults)
        {
            ValidationResults = validationResults;
        }
    }
}