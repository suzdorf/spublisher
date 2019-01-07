using SPublisher.Core.DbManagement;

namespace SPublisher.DBManagement.Models
{
    public class ScriptHashModel : IScriptHashInfo
    {
        public string Hash { get; set; }
    }
}