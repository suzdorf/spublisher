using System.Linq;
using Newtonsoft.Json;
using SPublisher.Configuration.Models;
using SPublisher.Core;
using SPublisher.Core.BuildSteps;
using SPublisher.Core.DbManagement;
using SPublisher.Core.Enums;

namespace SPublisher.Configuration.BuildSteps
{
    public class SqlStepModel : BuildStepModel, ISqlStep
    {
        public string ConnectionString { get; set; }
        public string ServerType { get; set; }
        public bool HashingEnabled { get; set; } = true;
        public DatabaseModel[] Databases { get; set; }
        IDatabase[] ISqlStep.Databases => Databases;

        [JsonIgnore]
        SqlServerType ISqlConnectionSettings.ServerType
        {
            get
            {
                var typeKeyValuePair = Constants.SqlServerType.BuildDictionary.FirstOrDefault(x => x.Value == ServerType);

                if (string.IsNullOrEmpty(ServerType))
                {
                    return SqlServerType.MsSql;
                }

                if (typeKeyValuePair.Value != null)
                {
                    return typeKeyValuePair.Key;
                }

                return SqlServerType.Invalid;
            }
        }
    }
}