

using Core.Common;

namespace OneTripMover.Core.Entity
{
    public class TypeSequentialIdGenerator<T> : IIdGenerator<T>
    {
        private readonly IIdGeneratorRegistry _idGenratorRegistry;
        
        public TypeSequentialIdGenerator()
        {
            _idGenratorRegistry = ServiceLocator.Resolve<IIdGeneratorRegistry>();
        }
        
        public EntityId<T> NewId() =>
            new (_idGenratorRegistry.NextFor<T>());

        public long PeekNext() =>
            _idGenratorRegistry.PeekFor<T>();
    }
}