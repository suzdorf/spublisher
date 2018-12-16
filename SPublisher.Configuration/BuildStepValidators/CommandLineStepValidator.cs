using System.Collections.Generic;
using SPublisher.Configuration.Exceptions;
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

        public IValidationError[] Validate(IBuildStep step)
        {
            var commandLineStep = (ICommandLineStep) step;

            if (commandLineStep.RunAsAdministrator && !_isAdministratorMode)
            {
                throw new ShouldRunAsAdministratorException();
            }

            return new IValidationError[0];
        }
    }
}