using System.Collections.Generic;
using System.Linq;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;

namespace SPublisher.Configuration
{
    public class ConfigurationValidator : IConfigurationValidator
    {
        private readonly IBuildStepValidatorFactory _validatorFactory;

        public ConfigurationValidator(IBuildStepValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public void Validate(IConfiguration configuration)
        {
            var validationResult = new List<IBuildStepValidationResult>();

            foreach (var buildStep in configuration.BuildSteps)
            {
                var validator = _validatorFactory.Get(buildStep);
                if (validator != null)
                {
                    validationResult.Add(validator.Validate(buildStep));
                }
            }

            if (validationResult.Any(x => x.Errors.Any()))
            {
                throw new ValidationException(validationResult);
            }
        }
    }
}