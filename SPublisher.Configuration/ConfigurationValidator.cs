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
            var validationResultList = new List<IBuildStepValidationResult>();

            foreach (var buildStep in configuration.BuildSteps)
            {
                var validator = _validatorFactory.Get(buildStep);
                if (validator != null)
                {
                    var validationResult = validator.Validate(buildStep);
                    validationResultList.Add(new BuildStepValidationResult(validationResult, buildStep));
                }
            }

            if (validationResultList.Any(x => x.Errors.Any()))
            {
                throw new ValidationException(validationResultList);
            }
        }
    }
}