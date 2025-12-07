using System.Threading;
using Core.MVP;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core.ModelProvider
{
    public class ModelProvider : IModelProvider
    {
        private int _currentId = -1;
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

        public int GetUniqueId()
        {
            _currentId++;
            return _currentId;
        }

        public async UniTask<TModel> GetAsync<TModel>() where TModel : IModel
        {
            var model = _objectResolver.Resolve<TModel>();
            _objectResolver.Inject(model);

            await InitModelAsync(model);

            return model;
        }

        public async UniTask InitModelAsync(IModel model)
        {
            await model.InitAsync();
        }
    }
}