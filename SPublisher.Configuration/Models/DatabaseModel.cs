using SPublisher.Core;

namespace SPublisher.Configuration.Models
{
    public class DatabaseModel : IDatabase
    {
        public string DatabaseName { get; set; }
        public bool HashingEnabled { get; set; } = true;
        public ScriptsModel[] Scripts { get; set; } =  new ScriptsModel[0];

        bool IDatabase.HashingEnabled
        {
            get
            {
                return HashingEnabled;
            }
        }

        IScripts[] IDatabase.Scripts => Scripts;
    }
}