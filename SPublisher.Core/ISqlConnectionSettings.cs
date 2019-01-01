namespace SPublisher.Core
{
    public interface ISqlConnectionSettings
    {
        string ConnectionString { get; }
        SqlServerType ServerType { get; }
    }
}