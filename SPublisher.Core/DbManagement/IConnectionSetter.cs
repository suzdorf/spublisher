namespace SPublisher.Core.DbManagement
{
    public interface IConnectionSetter
    {
        void Set(ISqlConnectionSettings connectionString);
    }
}