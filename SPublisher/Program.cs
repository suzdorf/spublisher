﻿using System;
using System.Security.Principal;
using SPublisher.BuildExecutor;
using SPublisher.Configuration;
using SPublisher.Core;

namespace SPublisher
{
    class Program
    {
        private static readonly IStorageAccessor StorageAccessor = new StorageAccessor();
        private static readonly IStorageLogger StorageLogger = new StorageLogger(StorageAccessor, LocalFolderPath);
        private static readonly ILogger Logger = new Logger(StorageLogger);
        // Build executor
        private static readonly IBuildStepExecutorFactory BuildStepExecutorFactory = new BuildStepExecutorFactory(BuildStepConfiguration.BuildStepExecutors);
        private static readonly IBuildExecutor BuildExecutor = new BuildExecutor.BuildExecutor(BuildStepExecutorFactory, Logger);

        // Configuration
        private static readonly IBuildStepValidatorFactory BuildStepValidatorFactory = new BuildStepValidatorFactory(BuildStepConfiguration.BuildStepValidators);
        private static readonly IConfigurationValidator ConfigurationValidator = new ConfigurationValidator(BuildStepValidatorFactory);
        private static readonly IRunOptionsFactory RunOptionsFactory = new RunOptionsFactory();
        private static readonly IConfigurationFactory ConfigurationFactory = new ConfigurationFactory(BuildStepConfiguration.BuildStepModelCreators, ConfigurationValidator);

        private static readonly SPublisherRunner Spublisher = new SPublisherRunner(
            RunOptionsFactory,
            Logger,
            StorageAccessor,
            BuildExecutor,
            ConfigurationFactory);

        static void Main(string[] args)
        {
            Logger.LogEvent(SPublisherEvent.SPublisherStarted);
            Spublisher.Run(args);
            Logger.LogEvent(SPublisherEvent.SPublisherCompleted);
        }

        public static bool IsAdministratorMode
        {
            get
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static string LocalFolderPath => $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\spublisher";
    }
}
