using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SPublisher.BuildExecutor.Exceptions;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor.BuildStepExecutors
{
    public class CommandLineExecutor : IBuildStepExecutor
    {
        private readonly ILogger _logger;

        public CommandLineExecutor(ILogger logger)
        {
            _logger = logger;
        }

        public ExecutionResult Execute(IBuildStep buildStep)
        {
            var step = (ICommandLineStep) buildStep;

            if (!step.Commands.Any())
                return ExecutionResult.Success;

            var command = string.Join(" && ", step.Commands);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd", "/c " + command)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            if (step.RunAsAdministrator)
            {
                process.StartInfo.Verb = "runas";
            }

            try
            {
                process.Start();
            }
            catch (Win32Exception ex)
            {
                _logger.LogError(ex);
                throw new CommandLineStartException();
            }

            var outputTask = Task.Run(() =>
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var output = process.StandardOutput.ReadLine();
                    _logger.LogOutput(output);
                }
            });

            var outputErrorTask = Task.Run(() =>
            {
                while (!process.StandardError.EndOfStream)
                {
                    var output = process.StandardError.ReadLine();
                    _logger.LogError(output);
                }
            });

            process.WaitForExit();
            Task.WhenAll(outputTask, outputErrorTask).Wait();
            return process.ExitCode == 0 ? ExecutionResult.Success : ExecutionResult.Error;
        }
    }
}