namespace OneTripMover.Core.Entity
{
    /// <summary>
    /// T型のID生成器
    /// </summary>
    public interface IIdGenerator<T>
    {
        /// <summary>
        /// 新しいEntityIdを返却する
        /// </summary>
        EntityId<T> NewId();
        
        /// <summary>
        /// 次に出る値
        /// </summary>
        long PeekNext();
    }
}