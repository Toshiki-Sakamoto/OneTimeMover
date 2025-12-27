namespace Core.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// out T をつけると、共変(covariant)になる。
    /// 戻り値にのみ使うことができる。派生型を派生元へそのまま代入できる
    /// </remarks>
    public interface ISubscriber<out T>
    {
        void Subscribe(System.Action<T> action);
    }
}