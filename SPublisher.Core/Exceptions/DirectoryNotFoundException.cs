using SPublisher.Core.Enums;
using SPublisher.Core.ExceptionMessages;

namespace SPublisher.Core.Exceptions
{
    public class DirectoryNotFoundException : SPublisherException, IDirectoryNotFoundMessage
    {
        public DirectoryNotFoundException(string path)
        {
            Path = path;
        }
        public override SPublisherEvent SPublisherEvent => SPublisherEvent.DirectoryNotFound;
        public string Path { get; }
    }
}