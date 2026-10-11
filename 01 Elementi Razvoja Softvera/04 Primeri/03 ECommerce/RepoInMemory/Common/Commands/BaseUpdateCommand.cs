using Application.Common.Commands;
using Domain;
using RepoInMemory.Common.DB;

namespace RepoInMemory.Common.Commands
{
    public class BaseUpdateCommand<TEntity> : IUpdateCommand<TEntity> where TEntity : Entity
    {
        public bool Update(TEntity entityToUpdate)
        {
            object id = entityToUpdate.GetId();
            if (InMemoryDatabase.Instance.Set<TEntity>().TryGetValue(id, out TEntity e))
            {
                return InMemoryDatabase.Instance.Set<TEntity>().TryUpdate(entityToUpdate.GetId(), entityToUpdate, e);
            }
            return false;
        }
    }
}
