using System;

namespace OneTripMover.Core.Entity
{
    /// <summary>
    /// エンティティ固有ID
    /// </summary>
    public readonly struct EntityId<T> : IEntityId, IEquatable<EntityId<T>>
    {
        public long Id { get; }
        
        public EntityId(long id) =>
            Id = id;

        public static bool operator ==(EntityId<T> left, EntityId<T> right) =>
            left.Id == right.Id;

        public static bool operator !=(EntityId<T> left, EntityId<T> right) => 
            !(left == right);

        public bool Equals(EntityId<T> other)
        {
            return Id == other.Id;
        }
        
        public bool Equals(IEntityId other)
        {
            if (other is EntityId<T> otherId)
            {
                return Equals(otherId);
            }
            
            return false;
        }

        public override bool Equals(object? obj)
        {
            return obj is EntityId<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}