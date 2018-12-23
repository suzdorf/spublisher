namespace SPublisher.Core
{
    public interface IDatabaseCreator
    {
        DatabaseCreateResult Create(IDatabaseCreate databaseCreate);
    }
}