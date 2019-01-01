using System;
using System.Collections.Generic;
using SPublisher.BuildExecutor;
using SPublisher.BuildExecutor.BuildStepExecutors;
using SPublisher.Configuration;
using SPublisher.Configuration.BuildSteps;
using SPublisher.Configuration.BuildStepValidators;
using SPublisher.Core;
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
        private static readonly IApplicationCreator ApplicationCreator = new ApplicationCreator(ServerManagerDataProvider, Logger);
        private static readonly ISiteCreator SiteCreator = new SiteCreator(ServerManagerAccessor, ApplicationCreator, Logger);

        // DB Management
        private static readonly DbConnection DbConnection = new DbConnection();
        private static readonly ISqlServerDataProviderFactory SqlServerDataProviderFactory =  new SqlServerDataProviderFactory(DbConnection);
        private static readonly IDatabaseCreator DatabaseCreator = new DatabaseCreator(SqlServerDataProviderFactory);
        private static readonly IScriptsExecutor ScriptsExecutor = new ScriptsExecutor(SqlServerDataProviderFactory, StorageAccessor, Logger);

        private const string CommandLineBuildStep = "cmd";
        private const string IisManagementBuildStep = "iis";
        private const string SqlBuildStep = "sql";

        public static readonly IDictionary<string, IBuildStepExecutor> BuildStepExecutors =
            new Dictionary<string, IBuildStepExecutor>
            {
                {CommandLineBuildStep, new CommandLineExecutor(Logger)},
                {IisManagementBuildStep, new IisManagementExecutor(SiteCreator, Logger)},
                {SqlBuildStep, new SqlExecutor(DatabaseCreator, Logger, DbConnection, ScriptsExecutor)}
            };

        public static readonly IDictionary<string, Func<BuildStepModel>> BuildStepModelCreators =
            new Dictionary<string, Func<BuildStepModel>>
            {
                {CommandLineBuildStep, () => new CommandLineStepModel()},
                {IisManagementBuildStep, () => new IisManagementStepModel()},
                {SqlBuildStep, () => new SqlStepModel()}
            };

        public static readonly IDictionary<string, IBuildStepValidator> BuildStepValidators =
            new Dictionary<string, IBuildStepValidator>
            {
                {CommandLineBuildStep, new CommandLineStepValidator(Program.IsAdministratorMode) },
                {IisManagementBuildStep, new IisManagementStepValidator(Program.IsAdministratorMode)},
                {SqlBuildStep, new SqlStepValidator()}
            };
    }
}