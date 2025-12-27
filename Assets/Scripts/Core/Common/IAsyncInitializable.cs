namespace Core.Common
{
    public interface IAsyncInitializable
    {
        System.Threading.Tasks.Task InitializeAsync();
    }
}