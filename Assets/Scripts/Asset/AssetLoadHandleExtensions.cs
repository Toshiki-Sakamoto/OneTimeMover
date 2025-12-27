using Core.Common;
using UnityEngine;

namespace OneTripMover.Asset
{
    /// <summary>
    /// IAssetLoadHandle を GameObject の寿命に紐付け、破棄時にReleaseする拡張。
    /// </summary>
    public static class AssetLoadHandleExtensions
    {
        public static THandle AddTo<THandle>(this THandle handle, GameObject owner)
            where THandle : class, IAssetLoadHandle<UnityEngine.Object>
        {
            if (handle == null || handle.Asset == null || owner == null)
            {
                return handle;
            }

            var service = ServiceLocator.Resolve<IAssetAutoReleaseService>();
            service?.Register(handle.Asset, owner);
            return handle;
        }

        public static THandle AddTo<THandle>(this THandle handle, UnityEngine.SceneManagement.Scene scene)
            where THandle : class, IAssetLoadHandle<UnityEngine.Object>
        {
            if (handle == null || handle.Asset == null || !scene.IsValid())
            {
                return handle;
            }

            var service = ServiceLocator.Resolve<IAssetAutoReleaseService>();
            service?.Register(handle.Asset, scene);
            return handle;
        }

        /// <summary>
        /// 現在のアクティブシーンに紐付け、シーンアンロード時にReleaseする。
        /// </summary>
        public static THandle AddToActiveScene<THandle>(this THandle handle)
            where THandle : class, IAssetLoadHandle<UnityEngine.Object>
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            return AddTo(handle, scene);
        }
    }

    /// <summary>
    /// GameObjectがDestroyされたときに紐付いたアセットをReleaseするフック。
    /// </summary>
    internal class AssetReleaseOnDestroyHook : MonoBehaviour
    {
        private UnityEngine.Object _asset;
        private IAssetLoader _loader;

        public void Register(UnityEngine.Object asset, IAssetLoader loader)
        {
            _asset = asset;
            _loader = loader;
        }

        private void OnDestroy()
        {
            if (_asset == null || _loader == null) return;

            if (_asset is GameObject go)
            {
                _loader.ReleaseInstance(go);
            }
            else
            {
                _loader.Release(_asset);
            }

            _asset = null;
            _loader = null;
        }
    }
}
