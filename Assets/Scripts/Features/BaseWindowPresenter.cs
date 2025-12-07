using Core.MVP;
using Core.WindowManager;

namespace Features
{
    public abstract class BaseWindowPresenter<TView, TModel> :
        BasePresenter<TView, TModel>,
        IWindowPresenter<TView, TModel>
        where TView : IWindowView
        where TModel : IModel
    {
    }
}