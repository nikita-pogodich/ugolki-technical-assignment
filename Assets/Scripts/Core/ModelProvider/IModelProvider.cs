using Core.MVP;
using Core.ServiceInitializer;
using Cysharp.Threading.Tasks;

namespace Core.ModelProvider
{
    public interface IModelProvider : IInitializableService, IScopedService
    {
        int GetUniqueId();
        UniTask<TModel> GetAsync<TModel>() where TModel : IModel;
        UniTask InitModelAsync(IModel model);
    }
}