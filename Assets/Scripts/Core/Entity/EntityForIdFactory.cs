using System.Reflection;
using Core.Common;

namespace OneTripMover.Core.Entity
{
    public class EntityForIdFactory<T, TEntity> : IEntityForIdFactory<T, TEntity>
        where TEntity : IEntity
        where T : IEntity, new()
    {
        private readonly IIdGenerator<TEntity> _idGenerator;
        
        public EntityForIdFactory()
        {
            _idGenerator = ServiceLocator.Resolve<IIdGenerator<TEntity>>();
        }

        public T Create()
        {
            var entity = new T();
            entity.AssignId(_idGenerator.NewId());

            return entity;
        }
    }
}