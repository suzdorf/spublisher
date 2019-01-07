using SPublisher.Core.DbManagement;

namespace SPublisher.Configuration.Models
{
    public class ScriptsModel : IScripts
    {
        public bool IsFolder { get; set; }
        public string Path { get; set; }
    }
}