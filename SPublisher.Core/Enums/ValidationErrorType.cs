namespace SPublisher.Core.Enums
{
    public enum ValidationErrorType
    {
        ApplicationPathValueIsRequired,
        ApplicationNameValueIsRequired,
        ApplicationChildrenShouldHaveUniqueNames,
        SitePortInvalidValue,
        SiteIpAddressInvalidValue,
        SiteHostNameHasInvalidValue,
        SqlStepConnectionStringIsRequired,
        SqlStepPathValueIsRequired,
        SqlServerTypeInvalidValue,
        DatabaseNameMustBeSpecifiedForRestoreOperation
    }
}