using System;
using System.Collections.Generic;
using SPublisher.BuildExecutor;
using SPublisher.BuildExecutor.BuildStepExecutors;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Core;
using SPublisher.IisManagement;

namespace SPublisher
{
    public static class BuildStepConfiguration
    {
        private static readonly ILogger Logger = new Logger();

        // IIS Management
        private static readonly ServerManagerAccessor ServerManagerAccessor = new ServerManagerAccessor();
        private static readonly IServerManagerDataProvider ServerManagerDataProvider = new ServerManagerDataProvider(ServerManagerAccessor);
        private static readonly IApplicationCreator ApplicationCreator = new ApplicationCreator(ServerManagerDataProvider, Logger);
        private static readonly ISiteCreator SiteCreator = new SiteCreator(ServerManagerAccessor, ApplicationCreator);

        private const string CommandLineBuildStep = "cmd";
        private const string BatchFileBuildStep = "bat";
        private const string IisManagementBuildStep = "iis";

        public static readonly IDictionary<string, IBuildStepExecutor> BuildStepExecutors =
            new Dictionary<string, IBuildStepExecutor>
            {
                {CommandLineBuildStep, new CommandLineExecutor()},
                {BatchFileBuildStep, new BatchFileExecutor()},
                {IisManagementBuildStep, new IisManagementExecutor(SiteCreator, Logger)}
            };

        public static readonly IDictionary<string, Func<BuildStepModel>> BuildStepModelCreators =
            new Dictionary<string, Func<BuildStepModel>>
            {
                {CommandLineBuildStep, () => new CommandLineStepModel()},
                {BatchFileBuildStep, () => new BatchFileStepModel()},
                {IisManagementBuildStep, () => new IisManagementStepModel()}
            };
    }
}