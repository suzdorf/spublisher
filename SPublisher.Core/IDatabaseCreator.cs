namespace SPublisher.Core
{
    public interface IDatabaseCreator
    {
        DatabaseCreateResult Create(IDatabase databaseCreate);
    }
}