using System.Collections.Generic;
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

            var scripts = new Dictionary<string, string>();

            foreach (var script in database.Scripts)
            {
                if (!script.IsFolder)
                {
                    scripts.Add(script.Path, _storageAccessor.ReadAllText(script.Path));
                }
                else
                {
                    foreach (var keyValuePair in _storageAccessor.ReadAllText(script.Path, SqlHelpers.SqlFileExtension))
                    {
                        scripts.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }

            foreach (var script in scripts)
            {
                _sqlServerDataProvider.ExecuteScript(script.Value, databaseName);
                _logger.LogEvent(SPublisherEvent.SqlScriptExecuted, new SqlScriptInfo(script.Key));
            }
        }
    }
}