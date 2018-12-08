using WixSharp;
using WixSharp.CommonTasks;

namespace Installer
{
    class Script
    {
        public static void Main(string[] args)
        {
            var pathVariable =
                new EnvironmentVariable("PATH", "[INSTALLDIR]");

            pathVariable.System = true;
            pathVariable.Action = EnvVarAction.set;
            pathVariable.Part = EnvVarPart.last;
            pathVariable.Permanent = true;

            var project = new Project("spublisher",
                new Dir(@"%ProgramFiles%\spublisher",
                    new DirFiles(@"spublisher\*.dll*"),
                    new File(@"spublisher\SPublisher.exe"),
                    new File(@"spublisher\SPublisher.exe.config")),
                pathVariable);

            project.SetVersionFromFile(@"spublisher\SPublisher.exe");
            project.LicenceFile = "licence.rtf";
            project.ScheduleReboot = new ScheduleReboot{ InstallSequence = RebootInstallSequence.Both};

            Compiler.BuildMsi(project);
        }
    }
}
