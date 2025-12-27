using System;

namespace OneTripMover.Core.Entity
{
    /// <summary>
    /// エンティティ基礎
    /// </summary>
    public interface IEntity : IEquatable<IEntity>
    {
        /// <summary>
        /// エンティティ固有ID
        /// </summary>
        public IEntityId Id { get; }

        /// <summary>
        /// IDの設定
        /// </summary>
        public void AssignId(IEntityId id);
        
        /// <summary>
        /// IDによる比較
        /// </summary>
        bool IEquatable<IEntity>.Equals(IEntity other) =>
            other.Id.Equals(Id);
    }
}