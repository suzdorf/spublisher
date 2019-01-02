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

        public DatabaseCreateResult Create(IDatabase databaseCreate)
        {
            if (_sqlServerDataProviderFactory.Get().DataBaseExists(databaseCreate.DatabaseName))
            {
                return DatabaseCreateResult.AlreadyExists;
            }

            _sqlServerDataProviderFactory.Get().CreateDataBase(databaseCreate);
            return DatabaseCreateResult.Success;
        }
    }
}