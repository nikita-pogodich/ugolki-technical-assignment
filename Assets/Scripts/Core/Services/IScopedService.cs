using VContainer;

namespace Core.Services
{
    public interface IScopedService
    {
        void SetContainer(IObjectResolver objectResolver);
    }
}