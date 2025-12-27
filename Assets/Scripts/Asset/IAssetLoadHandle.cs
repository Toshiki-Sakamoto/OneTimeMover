namespace OneTripMover.Asset
{
    public interface IAssetLoadHandle<out T>
    {
        T Asset { get; }
    }
}