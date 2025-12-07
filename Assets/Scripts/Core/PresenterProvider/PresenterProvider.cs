using System.Threading;
using Core.MVP;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core.PresenterProvider
{
    public class PresenterProvider : IPresenterProvider
    {
        private IObjectResolver _objectResolver = null;

        public bool IsInitialized { get; private set; } = false;

        public void SetContainer(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask InitializeAsync(CancellationToken cancellation)
        {
            await UniTask.WaitUntil(() => _objectResolver != null, cancellationToken: cancellation);

            IsInitialized = true;
        }

        public TPresenter Get<TPresenter, TView, TModel>(TView view, TModel model)
            where TPresenter : IPresenter<TView, TModel>
            where TView : IView
            where TModel : IModel
        {
            var presenter = _objectResolver.Resolve<TPresenter>();
            presenter.Init(view, model);
            return presenter;
        }
    }
}