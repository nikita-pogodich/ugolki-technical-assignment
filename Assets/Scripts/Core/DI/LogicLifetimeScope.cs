using Core.ModelProvider;
using Features.UgolkiLogic;
using VContainer;

namespace Core.DI
{
    public class LogicLifetimeScope : BaseLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterInitializableService<IModelProvider, ModelProvider.ModelProvider>(builder);
            builder.Register<IUgolkiModel, UgolkiModel>(Lifetime.Singleton);
        }
    }
}