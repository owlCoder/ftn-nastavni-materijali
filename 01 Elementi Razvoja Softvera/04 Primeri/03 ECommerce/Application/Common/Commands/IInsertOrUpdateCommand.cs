using Domain;

namespace Application.Common.Commands
{
    public interface IInsertOrUpdateCommand<TEntity> where TEntity : Entity
    {
        bool InsertOrUpdate(TEntity entity);
    }
}
