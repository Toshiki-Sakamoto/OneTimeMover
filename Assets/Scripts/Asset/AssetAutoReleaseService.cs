using Core.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneTripMover.Asset
{
    public class AssetAutoReleaseService : IAssetAutoReleaseService
    {
        private readonly IAssetLoader _assetLoader;

        public AssetAutoReleaseService()
        {
            _assetLoader = ServiceLocator.Resolve<IAssetLoader>();
        }

        public void Register(Object asset, GameObject owner)
        {
            if (asset == null || owner == null || _assetLoader == null) return;

            var hook = owner.GetComponent<AssetReleaseOnDestroyHook>();
            if (hook == null)
            {
                hook = owner.AddComponent<AssetReleaseOnDestroyHook>();
            }

            hook.Register(asset, _assetLoader);
        }

        public void Register(Object asset, Scene scene)
        {
            if (asset == null || !_assetLoaderValid || !scene.IsValid()) return;

            var hook = GetSceneHook(scene);
            hook.Register(asset, _assetLoader);
        }

        private bool _assetLoaderValid => _assetLoader != null;

        private AssetReleaseOnDestroyHook GetSceneHook(Scene scene)
        {
            // シーンに紐づけた隠しゲームオブジェクトを用意し、アンロード時に破棄されるようにする
            var roots = scene.GetRootGameObjects();
            foreach (var go in roots)
            {
                var existing = go.GetComponent<AssetReleaseOnDestroyHook>();
                if (existing != null)
                {
                    return existing;
                }
            }

            var hookObj = new GameObject("__AssetReleaseOnSceneUnload");
            hookObj.hideFlags = HideFlags.HideAndDontSave;
            SceneManager.MoveGameObjectToScene(hookObj, scene);
            return hookObj.AddComponent<AssetReleaseOnDestroyHook>();
        }
    }
}
