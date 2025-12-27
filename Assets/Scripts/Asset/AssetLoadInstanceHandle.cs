using UnityEngine;

namespace OneTripMover.Asset
{
    public class AssetLoadInstanceHandle<T> : IAssetLoadHandle<T> 
        where T : Object
    {
        public T Asset { get; }
        
        public AssetLoadInstanceHandle(T asset)
        {
            Asset = asset;
        }
    }
}