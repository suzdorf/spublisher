namespace SPublisher.Core
{
    public interface IProcessOutputLogger
    {
        void LogOutput(string output);

        void LogError(string error);
    }
}