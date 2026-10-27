using Application.Common.Commands;
using Domain;
using RepoInMemory.Common.DB;

namespace RepoInMemory.Common.Commands
{
    public abstract class BaseInsertCommand<TEntity> : IInsertCommand<TEntity> where TEntity : Entity
    {
        public bool Insert(TEntity entity)
        {
            return InMemoryDatabase.Instance.Set<TEntity>().TryAdd(entity.GetId(), entity);
        }
    }
}
