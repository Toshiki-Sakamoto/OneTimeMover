namespace Core.Common
{
    public interface IInjector
    {
        void Inject(object instance, IServiceResolver resolver);
    }
}