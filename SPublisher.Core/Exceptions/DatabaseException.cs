using SPublisher.Core.Enums;
using SPublisher.Core.ExceptionMessages;

namespace SPublisher.Core.Exceptions
{
    public class DatabaseException : SPublisherException, IDatabaseErrorMessage
    {
        public DatabaseException(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public override SPublisherEvent SPublisherEvent => SPublisherEvent.DatabaseError;
        public string ErrorMessage { get; }
    }
}