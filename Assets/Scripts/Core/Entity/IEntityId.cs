using System;

namespace OneTripMover.Core.Entity
{
    public interface IIdentifier
    {
        long Id { get; }
    }
    
    /// <summary>
    /// EntityID基底インターフェース
    /// </summary>
    public interface IEntityId : IEquatable<IEntityId>, IIdentifier
    {
    }
}