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
        IisManagementStarted,
        IisManagementCompleted,
        ApplicationPoolExists,
        ApplicationPoolCreated,
        SiteExists,
        SiteCreated,
        ApplicationExists,
        ApplicationCreated
    }
}