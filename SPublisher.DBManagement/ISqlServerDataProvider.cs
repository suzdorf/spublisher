using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public interface ISqlServerDataProvider
    {
        bool DataBaseExists(string dbName);

        void CreateDataBase(IDatabaseCreate databaseCreate);
    }
}