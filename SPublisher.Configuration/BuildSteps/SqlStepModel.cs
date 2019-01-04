using Newtonsoft.Json;
using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;

namespace SPublisher.Configuration.BuildSteps
{
    public class SqlStepModel : BuildStepModel, ISqlStep
    {
        public string ConnectionString { get; set; }
        public string ServerType { get; set; }
        public bool HashingEnabled { get; set; } = true;

        [JsonIgnore]
        SqlServerType ISqlConnectionSettings.ServerType
        {
            get
            {
                if (string.IsNullOrEmpty(ServerType) || ServerType.ToLower() == Constants.SqlServerType.MsSql)
                {
                    return SqlServerType.MsSql;
                }

                if (ServerType.ToLower() == Constants.SqlServerType.MySql)
                {
                    return SqlServerType.MySql;
                }

                if (ServerType.ToLower() == Constants.SqlServerType.PostgreSql)
                {
                    return SqlServerType.PostgreSql;
                }

                return SqlServerType.Invalid;
            }
        }

        public DatabaseModel[] Databases { get; set; }
        IDatabase[] ISqlStep.Databases => Databases;
    }
}