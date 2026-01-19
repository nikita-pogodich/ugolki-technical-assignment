using Core.ServiceInitialization;
using Core.Services;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class BaseLifetimeScope : LifetimeScope
    {
        protected void RegisterInitializableService<TInterface, TType>(IContainerBuilder builder)
            where TInterface : IInitializableService
            where TType : class, TInterface
        {
            builder.Register<TInterface, TType>(Lifetime.Singleton);
            builder.RegisterBuildCallback(
                container =>
                {
                    var serviceInitializer = container.Resolve<IServiceInitializer>();
                    var initializableService = container.Resolve<TInterface>();
                    serviceInitializer.AddService(initializableService);
                });
        }

        protected void SetScopedServiceContainer<TInterface>(IContainerBuilder builder)
            where TInterface : IScopedService
        {
            builder.RegisterBuildCallback(
                container =>
                {
                    var scopedService = container.Resolve<TInterface>();
                    scopedService.SetContainer(container);
                });
        }
    }
}