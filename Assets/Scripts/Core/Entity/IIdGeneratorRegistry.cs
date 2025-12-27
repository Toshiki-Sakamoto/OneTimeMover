namespace OneTripMover.Core.Entity
{
    /// <summary>
    /// IdGeneratorレジストリ
    /// </summary>
    public interface IIdGeneratorRegistry
    {
        /// <summary>
        /// 次のT型のIDを取り出す
        /// </summary>
        long NextFor<T>();
        
        /// <summary>
        /// 次のT型のIDを取り出す
        /// </summary>
        long PeekFor<T>();
    }
}
