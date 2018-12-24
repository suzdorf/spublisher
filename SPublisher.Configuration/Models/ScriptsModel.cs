using SPublisher.Core;

namespace SPublisher.Configuration.Models
{
    public class ScriptsModel : IScripts
    {
        public string Type { get; set; } = "file";
        public string Path { get; set; }
    }
}