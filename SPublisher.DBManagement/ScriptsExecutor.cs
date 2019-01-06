using System.Collections.Generic;
using System.Linq;
using SPublisher.Core;
using SPublisher.Core.Enums;

namespace SPublisher.DBManagement
{
    public class ScriptsExecutor : IScriptsExecutor
    {
        private readonly ISqlServerDataProviderFactory _sqlServerDataProviderFactory;
        private readonly IStorageAccessor _storageAccessor;
        private readonly ILogger _logger;

        public ScriptsExecutor(ISqlServerDataProviderFactory sqlServerDataProviderFactory, IStorageAccessor storageAccessor, ILogger logger)
        {
            _sqlServerDataProviderFactory = sqlServerDataProviderFactory;
            _storageAccessor = storageAccessor;
            _logger = logger;
        }

        public void ExecuteScripts(IDatabase database)
        {
            var hashingEnabled = database.HashingEnabled && !string.IsNullOrEmpty(database.DatabaseName);

            var dataProvider = _sqlServerDataProviderFactory.Get();
            if (hashingEnabled)
            {
                dataProvider.CreateHashInfoTableIfNotExists(database.DatabaseName);
            }

            var scripts = new List<IFile>();

            var excludedScripts = hashingEnabled
                ? dataProvider.GetHashInfoList(database.DatabaseName)
                : new IScriptHashInfo[0];

            foreach (var script in database.Scripts)
            {
                if (!script.IsFolder)
                {
                    scripts.Add(_storageAccessor.GetFile(script.Path));
                }
                else
                {
                   scripts.AddRange(_storageAccessor.GetFiles(script.Path, SqlHelpers.SqlFileExtension));
                }
            }

            foreach (var script in scripts.Where(x => !excludedScripts.Any() || excludedScripts.All(y => y.Hash != x.Hash)))
            {
                dataProvider.ExecuteScript(script.Content, database.DatabaseName);
                _logger.LogEvent(SPublisherEvent.SqlScriptExecuted, new SqlScriptInfo(script.Path));

                if (hashingEnabled)
                    dataProvider.SaveHashInfo(database.DatabaseName, script);
            }
        }
    }
}