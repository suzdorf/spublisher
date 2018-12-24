using System.Linq;
using SPublisher.Core;

namespace SPublisher.DBManagement
{
    public class ScriptsExecutor : IScriptsExecutor
    {
        private readonly ISqlServerDataProvider _sqlServerDataProvider;
        private readonly IStorageAccessor _storageAccessor;
        private readonly ILogger _logger;

        public ScriptsExecutor(ISqlServerDataProvider sqlServerDataProvider, IStorageAccessor storageAccessor, ILogger logger)
        {
            _sqlServerDataProvider = sqlServerDataProvider;
            _storageAccessor = storageAccessor;
            _logger = logger;
        }

        public void ExecuteScripts(IDatabase database)
        {
            var databaseName = string.IsNullOrEmpty(database.DatabaseName) ? SqlHelpers.MasterDatabaseName : database.DatabaseName;

            var scripts = database.Scripts.ToDictionary(x => x.Path, x => _storageAccessor.ReadAllText(x.Path));

            foreach (var script in scripts)
            {
                _sqlServerDataProvider.ExecuteScript(script.Value, databaseName);
                _logger.LogEvent(SPublisherEvent.SqlScriptExecuted, new SqlScriptInfo(script.Key));
            }
        }
    }
}