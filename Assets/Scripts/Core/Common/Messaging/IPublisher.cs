namespace Core.Common.Messaging
{
    /// <summary>
    /// メッセージを送信するPublisherのインターフェース
    /// </summary>
    /// <remarks>
    ///  in T : 反共変: Tの派生型も代入が可能になる 
    /// IPublisher<object> objPub = /* 実装 */;
    /// IPublisher<string> strPub = objPub;  // OK (object は string の親型)
    /// </remarks>
    public interface IPublisher<in T> 
    {
        void Publish(T message);
    }
}