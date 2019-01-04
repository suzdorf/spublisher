using Newtonsoft.Json;
using SPublisher.Core;

namespace SPublisher.Configuration.Models
{
    public class DatabaseModel : IDatabase
    {
        public string DatabaseName { get; set; }
        public string BackupPath { get; set; }
        [JsonIgnore]
        public bool RestoreAvailable { get; set; }
        public bool HashingEnabled { get; set; } = true;
        public ScriptsModel[] Scripts { get; set; } =  new ScriptsModel[0];
        IScripts[] IDatabase.Scripts => Scripts;
    }
}