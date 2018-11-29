using System;
using System.Diagnostics;
using SPublisher.Core.BuildSteps;

namespace SPublisher.BuildExecutor.BuildStepExecutors
{
    public class CommandLineExecutor : IBuildStepExecutor
    {
        public void Execute(IBuildStep buildStep)
        {
            var step = (ICommandLineStep) buildStep;
            var command = string.Join(" && ", step.Commands);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd", "/c " + command)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
            var result = process.StandardOutput.ReadToEnd();
            Console.WriteLine(result);
        }
    }
}