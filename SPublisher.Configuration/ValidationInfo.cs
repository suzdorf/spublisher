using System;
using System.Collections.Generic;
using System.Linq;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.Enums;

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
                {ValidationErrorType.SqlServerTypeInvalidValue, (step, data) => $"'ServerType' contains invalid value in the build step '{step.Name}'." },
                {ValidationErrorType.DatabaseNameMustBeSpecifiedForRestoreOperation, (step, data) => $"'BackupPath' contains a value in the build step '{step.Name}'. But the 'DatabaseName' value is empty. You should specify it in order to proceed with restore operation."},
                {ValidationErrorType.SiteHostNameHasInvalidValue, (step, data) => $"Build step \'{step.Name}\' contains incorrect \'HostName\' value. The host name must use a valid host name format and cannot contain the following characters: \"/\\[]:|<>+=;,?*$%#@{{}}^`. Example: www.spublisher.com." },
                {ValidationErrorType.SitePortInvalidValue, (step, data) => $"Build step \'{step.Name}\' contains incorrect \'Port\' value. The server port number must be a positive integer between 1 and 65535." },
                {ValidationErrorType.SiteIpAddressInvalidValue, (step, message) => $"Build step \'{step.Name}\' contains incorrect \'IpAddress\' value." }
            };
    }
}