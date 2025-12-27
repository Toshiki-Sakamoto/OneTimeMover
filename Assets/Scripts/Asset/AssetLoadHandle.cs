namespace OneTripMover.Asset
{
    public class AssetLoadHandle<T> : IAssetLoadHandle<T>
    {
        public T Asset { get; }

        public AssetLoadHandle(T asset)
        {
            Asset = asset;
        }
    }
}