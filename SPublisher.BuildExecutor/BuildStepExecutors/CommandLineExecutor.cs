using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;
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
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            if (step.RunAsAdministrator)
            {
//                if (!IsAdministrator())
//                {
//                    throw new Exception("You should run as administrator.");
//                }
                process.StartInfo.Verb = "runas";
            }

            process.Start();

            var outputTask = Task.Run(() =>
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var output = process.StandardOutput.ReadLine();
                    Console.WriteLine(output);
                }
            });

            var outputErrorTask = Task.Run(() =>
            {
                while (!process.StandardError.EndOfStream)
                {
                    var output = process.StandardError.ReadLine();
                    Console.WriteLine(output);
                }
            });

            process.WaitForExit();
            Task.WhenAll(outputTask, outputErrorTask).Wait();
            Console.WriteLine("Process exited with code: {0}", process.ExitCode);
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}