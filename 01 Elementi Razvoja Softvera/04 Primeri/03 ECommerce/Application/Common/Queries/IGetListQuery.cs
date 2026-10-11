using Domain;
using System.Collections.Generic;

namespace Application.Common.Queries
{
    public interface IGetListQuery<TEntity> where TEntity : Entity
    {
        List<TEntity> GetList();
    }
}
