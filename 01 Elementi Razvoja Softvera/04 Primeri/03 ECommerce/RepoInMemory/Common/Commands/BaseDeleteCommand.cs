using Application.Common.Commands;
using Domain;
using RepoInMemory.Common.DB;

namespace RepoInMemory.Common.Commands
{
    public abstract class BaseDeleteCommand<TEntity> : IDeleteCommand<TEntity> where TEntity : Entity
    {
        public bool Delete(object id)
        {
            return InMemoryDatabase.Instance.Set<TEntity>().TryRemove(id, out _);
        }
    }
}
