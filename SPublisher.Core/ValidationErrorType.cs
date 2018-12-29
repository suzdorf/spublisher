namespace SPublisher.Core
{
    public enum ValidationErrorType
    {
        ApplicationPathValueIsRequired,
        ApplicationNameValueIsRequired,
        ApplicationChildrenShouldHaveUniqueNames,
        SqlStepConnectionStringIsRequired,
        SqlStepPathValueIsRequired
    }
}