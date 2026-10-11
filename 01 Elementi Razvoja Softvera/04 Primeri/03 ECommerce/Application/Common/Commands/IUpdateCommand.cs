using Domain;

namespace Application.Common.Commands
{
    public interface IUpdateCommand<TEntity> where TEntity : Entity
    {
        bool Update(TEntity entityToUpdate);
    }
}
