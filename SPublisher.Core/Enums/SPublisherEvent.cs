﻿namespace SPublisher.Core.Enums
{
    public enum SPublisherEvent
    {
        SPublisherStarted,
        SPublisherCompleted,
        BuildExecutionStarted,
        BuildExecutionCompleted,
        BuildStepExecutionStarted,
        BuildStepExecutionCompleted,
        BuildStepExecutionCompletedWithError,
        BuildStepWasSkipped,
        IisManagementStarted,
        IisManagementCompleted,
        DatabaseCreationStarted,
        ScriptsExecutionStarted,
        ScriptsExecutionCompleted,
        ApplicationPoolExists,
        ApplicationPoolCreated,
        DatabaseExists,
        DatabaseCreated,
        SiteExists,
        SiteCreated,
        ApplicationExists,
        ApplicationCreated,
        VirtualDirectoryExists,
        VirtualDirectoryCreated,
        ApplicationListIsEmpty,
        InvalidJson,
        UnknownError,
        BuildStepTypeNotFound,
        BuildStepTypeIsMissing,
        CommandLineCouldNotStart,
        ShouldRunAsAdministrator,
        ValidationErrors,
        FileNotFound,
        DirectoryNotFound,
        SqlScriptExecuted,
        DatabaseError,
        DatabaseRestorationStarted,
        DatabaseRestored,
        InvalidConnectionStringFormat,
        BindingAlreadyExists,
        BindingAdded
    }
}