using System;
using System.Collections.Generic;
using SPublisher.Configuration;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

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
                {SPublisherEvent.ApplicationPoolExists, message => $"application pool with name '{((IAppPoolInfo) message).AppPoolName}' already exists"},
                {SPublisherEvent.ApplicationPoolCreated, message => $"application pool '{((IAppPoolInfo) message).AppPoolName}' created"},
                {SPublisherEvent.SiteExists, message => $"Site with name '{((IApplicationInfo) message).Name}' already exists"},
                {SPublisherEvent.SiteCreated, message => $"Site '{((IApplicationInfo) message).AppPoolName}' created"},
                {SPublisherEvent.ApplicationExists, message => $"Application with name '{((IApplicationInfo) message).Name}' already exists"},
                {SPublisherEvent.ApplicationCreated, message =>$"Application '{((IApplicationInfo) message).AppPoolName}' created"},
                {SPublisherEvent.ApplicationListIsEmpty, message => "'Applications' parameter is empty"},
                {SPublisherEvent.InvalidJson, message => "Application exited with error because 'spublisher.json' has invalid json format."},
                {SPublisherEvent.SpublisherJsonNotFound, message => "Application exited with error because 'spublisher.json' was not found."},
                {SPublisherEvent.UnknownError, message => "Application exited due to uknown error."},
                {SPublisherEvent.BuildStepTypeNotFound, message => $"spublisher.json contains build step with unknown type '{((IBuildStepTypeNotFoundMessage)message).Type}'. Change it to valid build step type." },
                {SPublisherEvent.BuildStepTypeIsMissing, message => "spublisher.json contains build step which misses the 'Type' field."},
                {SPublisherEvent.CommandLineCouldNotStart, message => "Could not run cmd since it is unavailable. Check your system configuration." },
                {SPublisherEvent.ShouldRunAsAdministrator, message => "You should run spublisher as administrator in order to execute some of the build steps" }
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