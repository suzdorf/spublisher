using System.Collections.Generic;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildStepValidators
{
    public class CommandLineStepValidator : IBuildStepValidator
    {
        private readonly bool _isAdministratorMode;

        public CommandLineStepValidator(bool isAdministratorMode)
        {
            _isAdministratorMode = isAdministratorMode;
        }

        public ValidationErrorType[] Validate(IBuildStep step)
        {
            var commandLineStep = (ICommandLineStep) step;
            var errors = new List<ValidationErrorType>();

            if (commandLineStep.RunAsAdministrator && !_isAdministratorMode)
            {
                errors.Add(ValidationErrorType.ShouldRunAsAdministrator);
            }

            return errors.ToArray();
        }
    }
}