using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OneTripMover.Asset
{
    public interface IAssetLoader
    {
        /// <summary>
        /// アセットを非同期で読み込む
        /// </summary>
        Task<IAssetLoadHandle<T>> LoadAssetAsync<T>(AssetReference assetReference, CancellationToken cancellationToken) where T : Object;
        
        /// <summary>
        /// 読み込み済みアセットを解放する
        /// </summary>
        void Release(Object asset);

        /// <summary>
        /// プレハブを非同期でロード＆インスタンス化する
        /// </summary>
        Task<IAssetLoadHandle<T>> LoadInstantiateAsync<T>(AssetReferenceGameObject prefabReference, Transform parent, bool instantiateInWorldSpace, CancellationToken cancellationToken) where T : Object;
        IAssetLoadHandle<T> LoadInstantiate<T>(AssetReferenceGameObject prefabReference, Transform parent, bool instantiateInWorldSpace) where T : Object;

        /// <summary>
        /// InstantiateAsyncで生成したインスタンスを解放（破棄）する
        /// </summary>
        void ReleaseInstance(GameObject instance);
    }
}
