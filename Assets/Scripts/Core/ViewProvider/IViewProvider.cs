using Core.MVP;
using Core.Services;
using Cysharp.Threading.Tasks;

namespace Core.ViewProvider
{
    public interface IViewProvider : IInitializableService, IScopedService
    {
        TView Get<TView>(string resourceKey) where TView : IView;
        UniTask<TView> GetAsync<TView>(string resourceKey) where TView : IView;
        void Release<TView>(string resourceKey, TView view) where TView : IView;
    }
}