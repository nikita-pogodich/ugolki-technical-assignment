using Core.MVP;
using Core.Services;
using Cysharp.Threading.Tasks;

namespace Core.WorldObjectManager
{
    public interface IWorldObjectManager : IInitializableService, IScopedService
    {
        UniTask CreateWorldObjectAsync<TPresenter, TView, TModel>(string viewName, TModel model)
            where TPresenter : IWorldObjectPresenter<TView, TModel>
            where TView : IWorldObjectView
            where TModel : IModel;

        void ReleaseWorldObject<TView, TModel>(TModel model)
            where TView : IWorldObjectView
            where TModel : IModel;
    }
}