using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public class DatabaseCreator : IDatabaseCreator
    {
        private readonly ISqlServerDataProvider _sqlServerDataProvider;

        public DatabaseCreator(ISqlServerDataProvider sqlServerDataProvider)
        {
            _sqlServerDataProvider = sqlServerDataProvider;
        }

        public DatabaseCreateResult Create(IDatabaseCreate databaseCreate)
        {
            if (_sqlServerDataProvider.DataBaseExists(databaseCreate.DbName))
            {
                return DatabaseCreateResult.AlreadyExists;
            }

            _sqlServerDataProvider.CreateDataBase(databaseCreate);
            return DatabaseCreateResult.Success;
        }
    }
}