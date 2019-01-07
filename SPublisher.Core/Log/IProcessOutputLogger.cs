namespace SPublisher.Core.Log
{
    public interface IProcessOutputLogger
    {
        void LogOutput(string output);

        void LogError(string error);
    }
}