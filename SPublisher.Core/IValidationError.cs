
using SPublisher.Core.Enums;

namespace SPublisher.Core
{
    public interface IValidationError
    {
        ValidationErrorType Type { get; }

        IValidationErrorData Data { get; }
    }
}