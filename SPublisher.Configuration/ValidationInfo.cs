using System;
using System.Collections.Generic;
using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public class ValidationInfo : IValidationInfo
    {
        private readonly IReadOnlyList<IBuildStepValidationResult> _validationResults;

        public ValidationInfo(IReadOnlyList<IBuildStepValidationResult> validationResults)
        {
            _validationResults = validationResults;
        }

        public string[] Errors
        {
            get
            {
                var errors = new List<string>();

                foreach (var result in _validationResults)
                {
                    errors.AddRange(result.Errors.Select(x => MessageDictionary[x](result.BuildStep)));
                }

                return errors.ToArray();
            }
        }

        private static readonly IDictionary<ValidationErrorType, Func<IBuildStep, string>> MessageDictionary =
            new Dictionary<ValidationErrorType, Func<IBuildStep, string>>
            {
                {ValidationErrorType.ShouldRunAsAdministrator, step => $"You should run spublisher as administrator to run '{step.Name}'. " }
            };
    }
}