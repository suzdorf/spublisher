﻿using System;
using System.Collections.Generic;
using System.IO;
using SPublisher.Configuration;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.DbManagement;
using SPublisher.Core.Enums;
using SPublisher.Core.ExceptionMessages;
using SPublisher.Core.IisManagement;
using SPublisher.Core.Log;
using SPublisher.DBManagement;

namespace SPublisher
{
    public class Logger : ILogger
    {
        private static readonly IDictionary<SPublisherEvent, Func<ILogMessage, string>> Messages =
            new Dictionary<SPublisherEvent, Func<ILogMessage, string>>
            {
                {SPublisherEvent.SPublisherStarted, message => "Spublisher started."},
                {SPublisherEvent.SPublisherCompleted, message => "Spublisher completed."},
                {SPublisherEvent.BuildExecutionStarted, message => "Build steps execution started."},
                {SPublisherEvent.BuildExecutionCompleted, message => "Build steps execution completed."},
                {
                    SPublisherEvent.BuildStepExecutionStarted, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"Build step with name '{buildStep.Name}' and of type '{buildStep.Type}' started.";
                    }
                },
                {
                    SPublisherEvent.BuildStepExecutionCompleted, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"Build step with name '{buildStep.Name}' and of type '{buildStep.Type}' completed.";
                    }
                },
                {
                SPublisherEvent.BuildStepExecutionCompletedWithError, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"Build step with name '{buildStep.Name}' and of type '{buildStep.Type}' has finished with error.";
                    }
                },
                {
                    SPublisherEvent.BuildStepWasSkipped, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"Build step with name '{buildStep.Name}' and of type '{buildStep.Type}' has been skipped.";
                    }
                },
                {SPublisherEvent.IisManagementStarted, message => "IIS site creation started."},
                {SPublisherEvent.IisManagementCompleted, message => "IIS site creation completed."},
                {SPublisherEvent.DatabaseCreationStarted, message => "Database creation started."},
                {SPublisherEvent.ApplicationPoolExists, message => $"Application pool with name '{((IAppPoolInfo) message).AppPoolName}' already exists."},
                {SPublisherEvent.ApplicationPoolCreated, message => $"Application pool '{((IAppPoolInfo) message).AppPoolName}' created."},
                {SPublisherEvent.SiteExists, message => $"Site with name '{((IApplicationInfo) message).Name}' already exists."},
                {SPublisherEvent.SiteCreated, message => $"Site '{((IApplicationInfo) message).Name}' created."},
                {SPublisherEvent.ApplicationExists, message => $"Application with name '{((IApplicationInfo) message).Name}' already exists."},
                {SPublisherEvent.ApplicationCreated, message =>$"Application '{((IApplicationInfo) message).Name}' created."},
                {SPublisherEvent.VirtualDirectoryExists, message => $"Virtual directory with name '{((IApplicationInfo) message).Name}' already exists."},
                {SPublisherEvent.VirtualDirectoryCreated, message =>$"Virtual directory '{((IApplicationInfo) message).Name}' created."},
                {SPublisherEvent.ApplicationListIsEmpty, message => "'Applications' parameter is empty."},
                {SPublisherEvent.InvalidJson, message => "Application exited with error because the file with your configuration has invalid json format."},
                {SPublisherEvent.FileNotFound, message => $"Application exited with error because file '{Path.GetFullPath(((IFileNotFoundMessage) message).Path)}' has not been found."},
                {SPublisherEvent.DirectoryNotFound, message => $"Application exited with error because directory '{Path.GetFullPath(((IDirectoryNotFoundMessage) message).Path)}' has not been found."},
                {SPublisherEvent.UnknownError, message => $"Application exited due to unknown error. For more information check logs in '{Program.LocalFolderPath}'"},
                {SPublisherEvent.BuildStepTypeNotFound, message => $"The file with your configuration contains build step with unknown type '{((IBuildStepTypeNotFoundMessage)message).Type}'. Change it to valid build step type." },
                {SPublisherEvent.BuildStepTypeIsMissing, message => "The file with your configuration contains build step which misses the 'Type' field."},
                {SPublisherEvent.CommandLineCouldNotStart, message => "Could not run cmd since it is unavailable. Check your system configuration." },
                {SPublisherEvent.ShouldRunAsAdministrator, message => "You should run spublisher as administrator in order to execute some of the build steps." },
                {SPublisherEvent.DatabaseExists, message => $"Database with the name '{((IDatabase) message).DatabaseName}' already exists."},
                {SPublisherEvent.DatabaseCreated, message => $"Database with the name '{((IDatabase) message).DatabaseName}' created." },
                {SPublisherEvent.ScriptsExecutionStarted, message => $"Execution scripts for database '{((IDatabase) message).DatabaseName ?? "master"}' started."},
                {SPublisherEvent.ScriptsExecutionCompleted, message => $"Execution scripts for database '{((IDatabase) message).DatabaseName ?? "master"}' completed." },
                {SPublisherEvent.SqlScriptExecuted, message => $"Sql script '{Path.GetFullPath(((ISqlScriptInfo) message).Path)}' executed." },
                {SPublisherEvent.DatabaseError, message => $"Application exited with error because Database exception has been thrown with message:{Environment.NewLine}{((IDatabaseErrorMessage) message).ErrorMessage}'." },
                {SPublisherEvent.DatabaseRestorationStarted, message => "Database restoration started." },
                {SPublisherEvent.DatabaseRestored, message => $"Database with the name '{((IDatabase) message).DatabaseName}' restored from backup file '{((IDatabase) message).BackupPath}'." },
                {SPublisherEvent.InvalidConnectionStringFormat, message => "The connection string provided has invalid format." },
                {
                    SPublisherEvent.BindingAdded, message =>
                    {
                        var binding = (IBinding) message;
                        return
                            $"Binding '{Constants.SiteBinding.Types.BuildDictionary[binding.Type]}:{binding.IpAddress}:{binding.Port}' with hostname '{binding.HostName}' has been added.";
                    }
                },
                {SPublisherEvent.CertificateNotFound, message => $"Certificate with the thumbprint'{((ICertificateNotFoundMessage)message).CertificateThumbprint}' has not been found." }
            };

        private readonly IStorageLogger _storageLogger;
        private readonly IConsoleLogger _consoleLogger;

        public Logger(IStorageLogger storageLogger, IConsoleLogger consoleLogger)
        {
            _storageLogger = storageLogger;
            _consoleLogger = consoleLogger;
        }

        public void LogEvent(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null)
        {
            _consoleLogger.LogInfo($"INFO: {Messages[sPublisherEvent](logMessage)}");
        }

        public void LogError(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null)
        {
            _consoleLogger.LogError($"ERROR: {Messages[sPublisherEvent](logMessage)}");
        }

        public void LogError(Exception exception)
        {
            _storageLogger.LogError(exception);
        }

        public void LogValidationError(IValidationInfo info)
        {
            _consoleLogger.LogError("VALIDATION ERROR: There are validation errors in you build configuration.");
            foreach (var error in info.Errors)
            {
                _consoleLogger.LogError($"VALIDATION ERROR: {error}");
            }
        }

        public void LogOutput(string output)
        {
            _consoleLogger.LogInfo(output);
        }

        public void LogError(string error)
        {
            _consoleLogger.LogError(error);
        }
    }
}