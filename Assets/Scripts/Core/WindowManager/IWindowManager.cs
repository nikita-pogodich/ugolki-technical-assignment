using Core.MVP;
using Core.Services;
using Cysharp.Threading.Tasks;

namespace Core.WindowManager
{
    public interface IWindowManager : IInitializableService, IScopedService
    {
        UniTask ShowWindowAsync<TView, TModel>(
            string viewName,
            WindowShowDelegate<TModel> beforeShow = null,
            WindowShowDelegate<TModel> alreadyShown = null)
            where TView : IView
            where TModel : IModel;

        void HideWindow(IModel model);
    }
}