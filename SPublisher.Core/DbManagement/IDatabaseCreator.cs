using SPublisher.Core.Enums;

namespace SPublisher.Core.DbManagement
{
    public interface IDatabaseCreator
    {
        DatabaseCreateResult Create(IDatabase database);
        void Restore(IDatabase database);
    }
}