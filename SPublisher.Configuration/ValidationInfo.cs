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
                    errors.AddRange(result.Errors.Select(x => MessageDictionary[x.Type](result.BuildStep, x.Data)));
                }

                return errors.ToArray();
            }
        }

        private static readonly IDictionary<ValidationErrorType, Func<IBuildStep, IValidationErrorData, string>> MessageDictionary =
            new Dictionary<ValidationErrorType, Func<IBuildStep, IValidationErrorData, string>>
            {
                {ValidationErrorType.ApplicationPathValueIsRequired, (step, data) => $"'Path' value is missing in one of the applications in the build step '{step.Name}'." },
                {ValidationErrorType.ApplicationNameValueIsRequired, (step, data) => $"'Name' value is missing in one of the applications in the build step '{step.Name}'." },
                {ValidationErrorType.ApplicationChildrenShouldHaveUniqueNames, (step, data) => $"Non unique 'Name' values have been found in application array of the build step '{step.Name}'." },
                {ValidationErrorType.SqlStepConnectionStringIsRequired, (step, data) => $"'ConnectionString' value is missing or empty in the build step '{step.Name}'." },
                {ValidationErrorType.SqlStepPathValueIsRequired, (step, data) => $"Some of the 'Path' values are null or empty in the build step '{step.Name}'. You should provide them." },
                {ValidationErrorType.SqlServerTypeInvalidValue, (step, data) => $"'ServerType' contains invalid value in the build step '{step.Name}'." }
            };
    }
}