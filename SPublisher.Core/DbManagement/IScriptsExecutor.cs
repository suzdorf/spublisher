namespace SPublisher.Core.DbManagement
{
    public interface IScriptsExecutor
    {
        void ExecuteScripts(IDatabase database);
    }
}