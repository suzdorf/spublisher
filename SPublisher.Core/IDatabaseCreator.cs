namespace SPublisher.Core
{
    public interface IDatabaseCreator
    {
        DatabaseCreateResult Create(IDatabase database);
        void Restore(IDatabase database);
    }
}