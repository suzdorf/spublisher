using SPublisher.Core;

namespace SPublisher.DBManagement.DataProviders
{
    public class MySqlDataProvider : ISqlServerDataProvider
    {
        public bool DataBaseExists(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public void CreateDataBase(IDatabase database)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteScript(string script, string databaseName)
        {
            throw new System.NotImplementedException();
        }
    }
}