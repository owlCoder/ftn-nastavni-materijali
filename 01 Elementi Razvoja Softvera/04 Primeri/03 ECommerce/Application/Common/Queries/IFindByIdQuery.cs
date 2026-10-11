using Domain;

namespace Application.Common.Queries
{
    public interface IFindByIdQuery<TEntity> where TEntity : Entity
    {
        TEntity FindById(object id);
    }
}
