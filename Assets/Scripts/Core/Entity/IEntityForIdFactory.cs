namespace OneTripMover.Core.Entity
{
    /// <summary>
    /// Entityファクトリ
    /// </summary>
    public interface IEntityForIdFactory<out T, TEntity>
        where TEntity : IEntity
        where T : IEntity, new()
    {
        T Create();
    }
}