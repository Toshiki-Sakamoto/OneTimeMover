
using System.Collections.Generic;

namespace OneTripMover.Master
{
    /// <summary>
    /// マスタデータレジストリクラス
    /// </summary>
    public interface IMasterDataRegistry<TMaster> where TMaster : IMasterData<TMaster>
    {
        /// <summary>
        /// マスタの登録
        /// </summary>
        void Register(TMaster master);
        
        /// <summary>
        /// マスタを全削除
        /// </summary>
        void Clear();

        /// <summary>
        /// マスタをIDから検索
        /// </summary>
        bool TryGetValue(MasterId<TMaster> id, out TMaster master);
    }
}
