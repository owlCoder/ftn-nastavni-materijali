using Application.Common.Queries;
using Domain;
using Domain.Helpers;
using RepoInMemory.Common.DB;

namespace RepoInMemory.Common.Queries
{
    public class BaseFindByIdQuery<TEntity> : IFindByIdQuery<TEntity> where TEntity : Entity
    {
        public TEntity FindById(object id)
        {
            if (InMemoryDatabase.Instance.Set<TEntity>().TryGetValue(id, out TEntity entity))
            {
                return (TEntity)entity;
            }
            return EmptyEntityBuilder.Instance.GetEmptyEntity<TEntity>();
        }
    }
}
