using System;
using System.Collections.Generic;
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
                {SPublisherEvent.IisManagementStarted, message => "iis site creation started"},
                {SPublisherEvent.IisManagementCompleted, message => "iis site creation completed"},
                {SPublisherEvent.ApplicationPoolExists, message => $"application pool with name '{((IAppPoolInfo) message).AppPoolName}' already exists"},
                {SPublisherEvent.ApplicationPoolCreated, message => $"application pool '{((IAppPoolInfo) message).AppPoolName}' created"},
                {SPublisherEvent.SiteExists, message => $"Site with name '{((IApplicationInfo) message).Name}' already exists"},
                {SPublisherEvent.SiteCreated, message => $"Site '{((IApplicationInfo) message).AppPoolName}' created"},
                {SPublisherEvent.ApplicationExists, message => $"Application with name '{((IApplicationInfo) message).Name}' already exists"},
                {SPublisherEvent.ApplicationCreated, message =>$"Application '{((IApplicationInfo) message).AppPoolName}' created"}
            };
        public void LogEvent(SPublisherEvent sPublisherEvent, ILogMessage logMessage = null)
        {
            Console.WriteLine($"INFO: {Messages[sPublisherEvent](logMessage)}");
        }
    }
}