using System.Reflection;
using Core.Common;

namespace OneTripMover.Core.Entity
{
    public class EntityForIdFactory<T, TEntity> : IEntityForIdFactory<T, TEntity>
        where TEntity : IEntity
        where T : IEntity, new()
    {
        private IIdGenerator<TEntity> _idGenerator;
        
        [Inject] 
        public void Construct(IIdGenerator<TEntity> idGenerator)
        {
            _idGenerator = idGenerator;
        }

        public T Create()
        {
            var entity = new T();
            entity.AssignId(_idGenerator.NewId());

            return entity;
        }
    }
}