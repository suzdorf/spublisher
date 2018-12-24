using SPublisher.Core;

namespace SPublisher.Configuration.Models
{
    public class ScriptsExecuteModel : ISciptsExecute
    {
        public string DatabaseName { get; set; }
        public string[] Scripts { get; set; }
    }
}