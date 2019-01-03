using SPublisher.Core;

namespace SPublisher.Configuration.Models
{
    public class FileModel : IFile
    {
        public string Path { get; set; }
        public string Hash { get; set; }
        public string Content { get; set; }
    }
}