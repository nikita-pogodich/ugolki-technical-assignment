using Core.WorldObjectManager;
using Features.UgolkiLogic;
using VContainer;

namespace Core.DI
{
    public class WorldObjectManagerLifetimeScope : BaseLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IWorldObjectFactory, UgolkiBoardFactory>(Lifetime.Singleton);

            SetScopedServiceContainer<IWorldObjectManager>(builder);
        }
    }
}