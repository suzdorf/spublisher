using System;
using System.Collections.Generic;
using SPublisher.BuildExecutor;
using SPublisher.BuildExecutor.BuildStepExecutors;
using SPublisher.Configuration;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.BuildStepValidators;
using SPublisher.Core;
using SPublisher.Core.IisManagement;
using SPublisher.DBManagement;
using SPublisher.IisManagement;

namespace SPublisher
{
    public static class BuildStepConfiguration
    {
        private static readonly IStorageAccessor StorageAccessor = new StorageAccessor();
        private static readonly IStorageLogger StorageLogger = new StorageLogger(StorageAccessor, Program.LocalFolderPath);
        private static readonly ILogger Logger = new Logger(StorageLogger);

        // IIS Management
        private static readonly ServerManagerAccessor ServerManagerAccessor = new ServerManagerAccessor();
        private static readonly IServerManagerDataProvider ServerManagerDataProvider = new ServerManagerDataProvider(ServerManagerAccessor);
        private static readonly IAppPoolCreator AppPoolCreator = new AppPoolCreator(ServerManagerDataProvider, Logger);
        private static readonly IApplicationCreator ApplicationCreator = new ApplicationCreator(ServerManagerDataProvider, Logger, AppPoolCreator);
        private static readonly IBindingsManager BindingsManager = new BindingsManager(ServerManagerDataProvider, Logger);
        private static readonly ISiteCreator SiteCreator = new SiteCreator(ServerManagerAccessor, ApplicationCreator, Logger, ServerManagerDataProvider, AppPoolCreator, BindingsManager);

        // DB Management
        private static readonly DbConnection DbConnection = new DbConnection();
        private static readonly ISqlServerDataProviderFactory SqlServerDataProviderFactory =  new SqlServerDataProviderFactory(DbConnection);
        private static readonly IDatabaseCreator DatabaseCreator = new DatabaseCreator(SqlServerDataProviderFactory);
        private static readonly IScriptsExecutor ScriptsExecutor = new ScriptsExecutor(SqlServerDataProviderFactory, StorageAccessor, Logger);
        private static readonly IDatabaseActionsExecutor DatabaseActionsExecutor =  new DatabaseActionsExecutor(DatabaseCreator,ScriptsExecutor, Logger);

        public static readonly IDictionary<string, IBuildStepExecutor> BuildStepExecutors =
            new Dictionary<string, IBuildStepExecutor>
            {
                {Constants.Steps.CommandLineBuildStep, new CommandLineExecutor(Logger)},
                {Constants.Steps.IisManagementBuildStep, new IisManagementExecutor(SiteCreator, Logger)},
                {Constants.Steps.SqlBuildStep, new SqlExecutor(DbConnection, DatabaseActionsExecutor)}
            };

        public static readonly IDictionary<string, Func<BuildStepModel>> BuildStepModelCreators =
            new Dictionary<string, Func<BuildStepModel>>
            {
                {Constants.Steps.CommandLineBuildStep, () => new CommandLineStepModel()},
                {Constants.Steps.IisManagementBuildStep, () => new IisManagementStepModel()},
                {Constants.Steps.SqlBuildStep, () => new SqlStepModel()}
            };

        public static readonly IDictionary<string, IBuildStepValidator> BuildStepValidators =
            new Dictionary<string, IBuildStepValidator>
            {
                {Constants.Steps.CommandLineBuildStep, new CommandLineStepValidator(Program.IsAdministratorMode) },
                {Constants.Steps.IisManagementBuildStep, new IisManagementStepValidator(Program.IsAdministratorMode)},
                {Constants.Steps.SqlBuildStep, new SqlStepValidator()}
            };
    }
}