using System.Collections.Generic;
using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor
{
    public class BuildStepExecutorFactory : IBuildStepExecutorFactory
    {
        private readonly IDictionary<string, IBuildStepExecutor> _buildStepExecutors;

        public BuildStepExecutorFactory(IDictionary<string, IBuildStepExecutor> buildStepExecutors)
        {
            _buildStepExecutors = buildStepExecutors;
        }

        public IBuildStepExecutor Get(IBuildStep buildStep)
        {
            return _buildStepExecutors[buildStep.Type];
        }
    }
}
