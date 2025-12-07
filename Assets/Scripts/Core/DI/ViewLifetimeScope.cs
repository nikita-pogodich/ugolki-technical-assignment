using Core.ViewProvider;
using VContainer;

namespace Core.DI
{
    public class ViewLifetimeScope : BaseLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            SetScopedServiceContainer<IViewProvider>(builder);
        }
    }
}