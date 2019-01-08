namespace SPublisher.Core.Log
{
    public interface IConsoleLogger
    {
        void LogInfo(string message);
        void LogError(string message);
    }
}