using SPublisher.Core;

namespace SPublisher.Configuration.Models
{
    public class DatabaseModel : IDatabase
    {
        public string DatabaseName { get; set; }
        public ScriptsModel[] Scripts { get; set; } =  new ScriptsModel[0];
        IScripts[] IDatabase.Scripts => Scripts;
    }
}