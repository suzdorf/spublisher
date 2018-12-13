using System.Collections.Generic;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration
{
    public class BuildStepValidatorFactory : IBuildStepValidatorFactory
    {
        private readonly IDictionary<string, IBuildStepValidator> _buildStepValidators;

        public BuildStepValidatorFactory(IDictionary<string, IBuildStepValidator> buildStepValidators)
        {
            _buildStepValidators = buildStepValidators;
        }

        public IBuildStepValidator Get(IBuildStep step)
        {
            return _buildStepValidators[step.Type];
        }
    }
}