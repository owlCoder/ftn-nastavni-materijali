using Domain;

namespace Application.Common.Commands
{
    public interface IInsertCommand<TEntity> where TEntity : Entity
    {
        bool Insert(TEntity entity);
    }
}
