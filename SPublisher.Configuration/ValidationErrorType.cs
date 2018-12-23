﻿namespace SPublisher.Configuration
{
    public enum ValidationErrorType
    {
        PathValueIsRequired,
        NameValueIsRequired,
        ApplicationChildrenShouldHaveUniqueNames,
        ConnectionStringIsRequired,
        DbNamesShouldBeUnique,
        DbNameIsRequired
    }
}