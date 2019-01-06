namespace SPublisher.Core.Enums
{
    public enum ValidationErrorType
    {
        ApplicationPathValueIsRequired,
        ApplicationNameValueIsRequired,
        ApplicationChildrenShouldHaveUniqueNames,
        SqlStepConnectionStringIsRequired,
        SqlStepPathValueIsRequired,
        SqlServerTypeInvalidValue,
        DatabaseNameMustBeSpecifiedForRestoreOperation
    }
}