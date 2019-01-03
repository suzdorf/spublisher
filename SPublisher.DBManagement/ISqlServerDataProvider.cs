using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public interface ISqlServerDataProvider
    {
        bool DataBaseExists(string databaseName);

        void CreateDataBase(IDatabase database);

        void ExecuteScript(string script, string databaseName);

        void CreateHashInfoTableIfNotExists(string databaseName);

        IScriptHashInfo[] GetHashInfoList(string databaseName);

        void SaveHashInfo(string databaseName, IFile hashInfo);
    }
}