using VContainer;

namespace Core.ServiceInitializer
{
    public interface IScopedService
    {
        void SetContainer(IObjectResolver objectResolver);
    }
}