﻿using SPublisher.Core;

namespace SPublisher.Configuration
{
    public class ValidationError : IValidationError
    {
        public ValidationError(ValidationErrorType type, IValidationErrorData data = null)
        {
            Type = type;
            Data = data;
        }

        public ValidationErrorType Type { get; }
        public IValidationErrorData Data { get; }
    }
}