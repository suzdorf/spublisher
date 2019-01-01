using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public class DatabaseCreator : IDatabaseCreator
    {
        private readonly ISqlServerDataProvider _sqlServerDataProvider;

        public DatabaseCreator(ISqlServerDataProviderFactory sqlServerDataProviderFactory)
        {
            _sqlServerDataProvider = sqlServerDataProviderFactory.Get();
        }

        public DatabaseCreateResult Create(IDatabase databaseCreate)
        {
            if (_sqlServerDataProvider.DataBaseExists(databaseCreate.DatabaseName))
            {
                return DatabaseCreateResult.AlreadyExists;
            }

            _sqlServerDataProvider.CreateDataBase(databaseCreate);
            return DatabaseCreateResult.Success;
        }
    }
}