
namespace SPublisher.Core
{
    public interface IValidationError
    {
        ValidationErrorType Type { get; }

        IValidationErrorData Data { get; }
    }
}