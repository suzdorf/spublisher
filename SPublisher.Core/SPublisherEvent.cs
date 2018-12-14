namespace SPublisher.Core
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
        ApplicationPoolExists,
        ApplicationPoolCreated,
        SiteExists,
        SiteCreated,
        ApplicationExists,
        ApplicationCreated,
        ApplicationListIsEmpty,
        InvalidJson,
        SpublisherJsonNotFound,
        UnknownError
    }
}