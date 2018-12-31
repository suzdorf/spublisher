namespace SPublisher.Core
{
    public interface IRunOptionsFactory
    {
        IRunOptions Get(string[] args);
    }
}