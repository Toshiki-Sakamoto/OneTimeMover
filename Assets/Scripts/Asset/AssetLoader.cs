using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace OneTripMover.Asset
{
    /// <summary>
    /// Addressablesベースの簡易アセットローダー。
    /// </summary>
    public class AssetLoader : IAssetLoader
    {
        public async Task<IAssetLoadHandle<T>> LoadAssetAsync<T>(AssetReference assetReference, CancellationToken cancellationToken)
            where T : Object
        {
            if (assetReference == null)
            {
                Debug.LogWarning("AssetLoader: assetReference is null.");
                return null;
            }

            var handle = assetReference.LoadAssetAsync<T>();
            await using (cancellationToken.Register(() =>
                         {
                             if (!handle.IsDone && handle.IsValid()) Addressables.Release(handle);
                         }))
            {
                try
                {
                    var asset = await handle.Task;
                    return new AssetLoadHandle<T>(asset);
                }
                catch
                {
                    if (handle.IsValid())
                    {
                        Addressables.Release(handle);
                    }

                    throw;
                }
            }
        }

        public async Task<IAssetLoadHandle<T>> LoadInstantiateAsync<T>(AssetReferenceGameObject prefabReference, Transform parent, bool instantiateInWorldSpace, CancellationToken cancellationToken)
            where T : Object
        {
            if (prefabReference == null)
            {
                Debug.LogWarning("AssetLoader: prefabReference is null.");
                return null;
            }

            var handle = prefabReference.InstantiateAsync(parent, instantiateInWorldSpace);
            await using (cancellationToken.Register(() =>
                         {
                             if (!handle.IsDone && handle.IsValid()) Addressables.Release(handle);
                         }))
            {
                try
                {
                    var instance = await handle.Task;
                    return new AssetLoadInstanceHandle<T>(instance.GetComponent<T>());
                }
                catch
                {
                    if (handle.IsValid())
                    {
                        Addressables.Release(handle);
                    }

                    throw;
                }
            }
        }

        public IAssetLoadHandle<T> LoadInstantiate<T>(AssetReferenceGameObject prefabReference, Transform parent, bool instantiateInWorldSpace) where T : Object
        {
            if (prefabReference == null)
            {
                Debug.LogWarning("AssetLoader: prefabReference is null.");
                return null;
            }

            var handle = prefabReference.InstantiateAsync(parent, instantiateInWorldSpace);
            {
                try
                {
                    var instance = handle.WaitForCompletion();
                    return new AssetLoadInstanceHandle<T>(instance.GetComponent<T>());
                }
                catch
                {
                    if (handle.IsValid())
                    {
                        Addressables.Release(handle);
                    }

                    throw;
                }
            }
        }

        public void Release(Object asset)
        {
            if (asset == null) return;
            Addressables.Release(asset);
        }

        public void ReleaseInstance(GameObject instance)
        {
            if (instance == null) return;
            Addressables.ReleaseInstance(instance);
        }
    }
}
