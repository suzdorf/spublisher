using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public class DatabaseCreator : IDatabaseCreator
    {
        private readonly ISqlServerDataProviderFactory _sqlServerDataProviderFactory;

        public DatabaseCreator(ISqlServerDataProviderFactory sqlServerDataProviderFactory)
        {
            _sqlServerDataProviderFactory = sqlServerDataProviderFactory;
        }

        public DatabaseCreateResult Create(IDatabase database)
        {
            if (_sqlServerDataProviderFactory.Get().DataBaseExists(database.DatabaseName))
            {
                return DatabaseCreateResult.AlreadyExists;
            }

            _sqlServerDataProviderFactory.Get().CreateDataBase(database);
            return DatabaseCreateResult.Success;
        }

        public void Restore(IDatabase database)
        {
            _sqlServerDataProviderFactory.Get().RestoreDatabase(database);
        }
    }
}