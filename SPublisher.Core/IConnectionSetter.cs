namespace SPublisher.Core
{
    public interface IConnectionSetter
    {
        void Set(ISqlConnectionSettings connectionString);
    }
}