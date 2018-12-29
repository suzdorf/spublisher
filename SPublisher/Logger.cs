using System;
using System.Collections.Generic;
using SPublisher.Configuration;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.ExceptionMessages;
using SPublisher.DBManagement;

namespace SPublisher
{
    public class Logger : ILogger
    {
        private static readonly IDictionary<SPublisherEvent, Func<ILogMessage, string>> Messages =
            new Dictionary<SPublisherEvent, Func<ILogMessage, string>>
            {
                {SPublisherEvent.SPublisherStarted, message => "spublisher started"},
                {SPublisherEvent.SPublisherCompleted, message => "spublisher completed"},
                {SPublisherEvent.BuildExecutionStarted, message => "build steps execution started"},
                {SPublisherEvent.BuildExecutionCompleted, message => "build steps execution completed"},
                {
                    SPublisherEvent.BuildStepExecutionStarted, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"build step with name '{buildStep.Name}' and of type '{buildStep.Type}' started";
                    }
                },
                {
                    SPublisherEvent.BuildStepExecutionCompleted, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"build step with name '{buildStep.Name}' and of type '{buildStep.Type}' completed";
                    }
                },
                {
                SPublisherEvent.BuildStepExecutionCompletedWithError, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"build step with name '{buildStep.Name}' and of type '{buildStep.Type}' has finished with error";
                    }
                },
                {
                    SPublisherEvent.BuildStepWasSkipped, message =>
                    {
                        var buildStep = (IBuildStep) message;
                        return
                            $"build step with name '{buildStep.Name}' and of type '{buildStep.Type}' was skipped";
                    }
                },
                {SPublisherEvent.IisManagementStarted, message => "iis site creation started"},
                {SPublisherEvent.IisManagementCompleted, message => "iis site creation completed"},
                {SPublisherEvent.DatabaseCreationStarted, message => "database creation started"},
                {SPublisherEvent.DatabaseCreationCompleted, message => "database creation completed"},
                {SPublisherEvent.ApplicationPoolExists, message => $"application pool with name '{((IAppPoolInfo) message).AppPoolName}' already exists"},
                {SPublisherEvent.ApplicationPoolCreated, message => $"application pool '{((IAppPoolInfo) message).AppPoolName}' created"},
                {SPublisherEvent.SiteExists, message => $"Site with name '{((IApplicationInfo) message).Name}' already exists"},
                {SPublisherEvent.SiteCreated, message => $"Site '{((IApplicationInfo) message).Name}' created"},
                {SPublisherEvent.ApplicationExists, message => $"Application with name '{((IApplicationInfo) message).Name}' already exists"},
                {SPublisherEvent.ApplicationCreated, message =>$"Application '{((IApplicationInfo) message).Name}' created"},
                {SPublisherEvent.VirtualDirectoryExists, message => $"Virtual directory with name '{((IApplicationInfo) message).Name}' already exists"},
                {SPublisherEvent.VirtualDirectoryCreated, message =>$"Virtual directory '{((IApplicationInfo) message).Name}' created"},
                {SPublisherEvent.ApplicationListIsEmpty, message => "'Applications' parameter is empty"},
                {SPublisherEvent.InvalidJson, message => "Application exited with error because 'spublisher.json' has invalid json format."},
                {SPublisherEvent.FileNotFound, message => $"Application exited with error because file '{((IFileNotFoundMessage) message).Path}' was not found."},
                {SPublisherEvent.DirectoryNotFound, message => $"Application exited with error because directory '{((IDirectoryNotFoundMessage) message).Path}' was not found."},
                {SPublisherEvent.UnknownError, message => "Application exited due to unknown error."},
                {SPublisherEvent.BuildStepTypeNotFound, message => $"spublisher.json contains build step with unknown type '{((IBuildStepTypeNotFoundMessage)message).Type}'. Change it to valid build step type." },
                {SPublisherEvent.BuildStepTypeIsMissing, message => "spublisher.json contains build step which misses the 'Type' field."},
                {SPublisherEvent.CommandLineCouldNotStart, message => "Could not run cmd since it is unavailable. Check your system configuration." },
                {SPublisherEvent.ShouldRunAsAdministrator, message => "You should run spublisher as administrator in order to execute some of the build steps" },
                {SPublisherEvent.DatabaseExists, message => $"Database with the name '{((IDatabase) message).DatabaseName}' already exists"},
                {SPublisherEvent.DatabaseCreated, message => $"Database with the name '{((IDatabase) message).DatabaseName}' created" },
                {SPublisherEvent.ScriptsExecutionStarted, message => $"Execution scripts for database '{((IDatabase) message).DatabaseName ?? "master"}' started"},
                {SPublisherEvent.ScriptsExecutionCompleted, message => $"Execution scripts for database '{((IDatabase) message).DatabaseName ?? "master"}' completed" },
                {SPublisherEvent.SqlScriptExecuted, message => $"Sql script '{((ISqlScriptInfo) message).Path}' executed  " },
                {SPublisherEvent.DatabaseError, message => $"Application exited with error because SqlException was thrown with message:{Environment.NewLine} {((IDatabaseErrorMessage) message).ErrorMessage}'" }
            };
        public void LogEvent(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null)
        {
            Console.WriteLine($"INFO: {Messages[sPublisherEvent](logMessage)}");
        }

        public void LogError(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null)
        {
            Console.WriteLine($"ERROR: {Messages[sPublisherEvent](logMessage)}");
        }

        public void LogValidationError(IValidationInfo info)
        {
            Console.WriteLine("VALIDATION ERROR: There are validation errors in you build configuration.");
            foreach (var error in info.Errors)
            {
                Console.WriteLine($"VALIDATION ERROR: {error}");
            }
        }

        public void LogOutput(string output)
        {
            Console.WriteLine(output);
        }

        public void LogError(string error)
        {
            Console.WriteLine(error);
        }
    }
}