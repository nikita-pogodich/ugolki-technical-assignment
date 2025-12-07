using Core.MVP;
using Core.ServiceInitializer;

namespace Core.PresenterProvider
{
    public interface IPresenterProvider : IInitializableService, IScopedService
    {
        TPresenter Get<TPresenter, TView, TModel>(TView view, TModel model)
            where TPresenter : IPresenter<TView, TModel>
            where TView : IView
            where TModel : IModel;
    }
}