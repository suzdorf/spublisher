namespace SPublisher.Configuration
{
    public interface IValidationError
    {
        ValidationErrorType Type { get; }

        IValidationErrorData Data { get; }
    }
}