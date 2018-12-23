using SPublisher.Core;

namespace SPublisher.Configuration.Models
{
    public class DatabaseCreateModel : IDatabaseCreate
    {
        public string DbName { get; set; }
    }
}