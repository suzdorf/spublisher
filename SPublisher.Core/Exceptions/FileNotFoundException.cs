using SPublisher.Core.Enums;
using SPublisher.Core.ExceptionMessages;

namespace SPublisher.Core.Exceptions
{
    public class FileNotFoundException : SPublisherException, IFileNotFoundMessage
    {
        public FileNotFoundException(string path)
        {
            Path = path;
        }
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.FileNotFound;
        public string Path { get; }
    }
}