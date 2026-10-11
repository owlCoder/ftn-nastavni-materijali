using Application.Common.Queries;
using Domain;
using RepoInMemory.Common.DB;
using System.Collections.Generic;
using System.Linq;

namespace RepoInMemory.Common.Queries
{
    public class BaseGetListQuery<TEntity> : IGetListQuery<TEntity> where TEntity : Entity
    {
        public List<TEntity> GetList()
        {
            return InMemoryDatabase.Instance.Set<TEntity>().Values.ToList();
        }
    }
}
