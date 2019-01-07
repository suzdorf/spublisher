namespace SPublisher.Core.DbManagement
{
    public interface IDatabaseActionsExecutor
    {
        void Execute(IDatabase database);
    }
}