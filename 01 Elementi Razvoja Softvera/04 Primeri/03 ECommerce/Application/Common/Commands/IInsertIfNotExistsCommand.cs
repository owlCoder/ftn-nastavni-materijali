using Domain;

namespace Application.Common.Commands
{
    public interface IInsertIfNotExistsCommand<TEntity> where TEntity : Entity
    {
        bool InsertIfNotExists(TEntity entity);
    }
}
