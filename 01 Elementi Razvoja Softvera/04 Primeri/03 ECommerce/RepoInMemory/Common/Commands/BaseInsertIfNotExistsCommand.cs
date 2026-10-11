using Application.Common.Commands;
using Application.Common.Queries;
using Domain;

namespace RepoInMemory.Common.Commands
{
    public abstract class BaseInsertIfNotExistsCommand<TEntity> : IInsertIfNotExistsCommand<TEntity> where TEntity : Entity
    {
        private IFindByIdQuery<TEntity> findByIdQuery;
        private IInsertCommand<TEntity> insertCommand;

        public BaseInsertIfNotExistsCommand(IFindByIdQuery<TEntity> findByIdQuery, IInsertCommand<TEntity> insertCommand)
        {
            this.findByIdQuery = findByIdQuery;
            this.insertCommand = insertCommand;
        }

        public bool InsertIfNotExists(TEntity entity)
        {
            TEntity t = FindById(entity.GetId());
            if (t == null)
            {
                return Insert(entity);
            }
            return false;
        }

        private TEntity FindById(object id)
        {
            return findByIdQuery.FindById(id);
        }

        private bool Insert(TEntity entity)
        {
            return this.insertCommand.Insert(entity);
        }
    }
}
