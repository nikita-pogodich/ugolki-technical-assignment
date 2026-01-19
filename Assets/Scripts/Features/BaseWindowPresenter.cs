using Core.MVP;

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