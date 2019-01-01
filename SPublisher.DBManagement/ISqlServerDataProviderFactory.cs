namespace SPublisher.DBManagement
{
    public interface ISqlServerDataProviderFactory
    {
        ISqlServerDataProvider Get();
    }
}