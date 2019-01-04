using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using Npgsql;
using SPublisher.Core;
using SPublisher.Core.Exceptions;

namespace SPublisher.DBManagement
{
    public class DatabaseActionsExecutor : IDatabaseActionsExecutor
    {
        private readonly IDatabaseCreator _databaseCreator;
        private readonly IScriptsExecutor _scriptsExecutor;
        private readonly ILogger _logger;

        public DatabaseActionsExecutor(IDatabaseCreator databaseCreator, IScriptsExecutor scriptsExecutor, ILogger logger)
        {
            _databaseCreator = databaseCreator;
            _scriptsExecutor = scriptsExecutor;
            _logger = logger;
        }

        public void Execute(IDatabase database)
        {
            try
            {
                if (!string.IsNullOrEmpty(database.DatabaseName))
                {
                    if (!string.IsNullOrEmpty(database.BackupPath))
                    {
                        _logger.LogEvent(SPublisherEvent.DatabaseRestorationStarted);
                        _databaseCreator.Restore(database);
                        _logger.LogEvent(SPublisherEvent.DatabaseRestored, database);
                    }
                    else
                    {
                        _logger.LogEvent(SPublisherEvent.DatabaseCreationStarted);

                        var result = _databaseCreator.Create(database);

                        if (result == DatabaseCreateResult.AlreadyExists)
                        {
                            _logger.LogEvent(SPublisherEvent.DatabaseExists, database);
                        }

                        if (result == DatabaseCreateResult.Success)
                        {
                            _logger.LogEvent(SPublisherEvent.DatabaseCreated, database);
                        }
                    }
                }

                if (database.Scripts.Any())
                {
                    _logger.LogEvent(SPublisherEvent.ScriptsExecutionStarted, database);
                    _scriptsExecutor.ExecuteScripts(database);
                    _logger.LogEvent(SPublisherEvent.ScriptsExecutionCompleted, database);
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseException(ex.Message);
            }
            catch (MySqlException ex)
            {
                throw new DatabaseException(ex.Message);
            }
            catch (PostgresException ex)
            {
                throw new DatabaseException(ex.Message);
            }
        }
    }
}