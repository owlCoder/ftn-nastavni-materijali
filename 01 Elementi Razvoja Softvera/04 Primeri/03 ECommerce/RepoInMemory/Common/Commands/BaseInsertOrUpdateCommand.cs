using Application.Common.Commands;
using Application.Common.Queries;
using Domain;

namespace RepoInMemory.Common.Commands
{
    public class BaseInsertOrUpdateCommand<TEntity> : IInsertOrUpdateCommand<TEntity> where TEntity : Entity
    {
        private IFindByIdQuery<TEntity> findByIdQuery;
        private IInsertCommand<TEntity> insertCommand;
        private IUpdateCommand<TEntity> updateCommand;

        public BaseInsertOrUpdateCommand(IFindByIdQuery<TEntity> findByIdQuery, IInsertCommand<TEntity> insertCommand, IUpdateCommand<TEntity> updateCommand)
        {
            this.findByIdQuery = findByIdQuery;
            this.insertCommand = insertCommand;
            this.updateCommand = updateCommand;
        }

        public bool InsertOrUpdate(TEntity entity)
        {
            TEntity t = FindById(entity.GetId());
            if (t == null)
            {
                return Insert(entity);
            }
            else
            {
                return Update(entity);

            }
        }

        private TEntity FindById(object id)
        {
            return findByIdQuery.FindById(id);
        }

        private bool Insert(TEntity entity)
        {
            return insertCommand.Insert(entity);
        }

        private bool Update(TEntity entity)
        {
            return updateCommand.Update(entity);
        }
    }
}
