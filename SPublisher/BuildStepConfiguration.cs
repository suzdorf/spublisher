using System;
using System.Collections.Generic;
using SPublisher.BuildExecutor;
using SPublisher.BuildExecutor.BuildStepExecutors;
using SPublisher.Configuration.BuildSteps;

namespace SPublisher
{
    public static class BuildStepConfiguration
    {
        private const string CommandLineBuildStep = "cmd";
        private const string BatchFileBuildStep = "bat";

        public static readonly IDictionary<string, IBuildStepExecutor> BuildStepExecutors =
            new Dictionary<string, IBuildStepExecutor>
            {
                {CommandLineBuildStep, new CommandLineExecutor()},
                {BatchFileBuildStep, new BatchFileExecutor()}
            };

        public static readonly IDictionary<string, Func<BuildStepModel>> BuildStepModelCreators =
            new Dictionary<string, Func<BuildStepModel>>
            {
                {CommandLineBuildStep, () => new CommandLineStepModel()},
                {BatchFileBuildStep, () => new BatchFileStepModel()}
            };
    }
}