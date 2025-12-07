using Core.WindowManager;
using Features.GameHUDWindow;
using Features.GameResultPopup;
using Features.MainMenu;
using Features.MessagePopup;
using VContainer;

namespace Core.DI
{
    public class WindowManagerLifetimeScope : BaseLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IWindowFactory, MainMenuWindowFactory>(Lifetime.Singleton);
            builder.Register<IWindowFactory, MessagePopupWindowFactory>(Lifetime.Singleton);
            builder.Register<IWindowFactory, GameResultPopupWindowFactory>(Lifetime.Singleton);
            builder.Register<IWindowFactory, GameHUDWindowFactory>(Lifetime.Singleton);

            SetScopedServiceContainer<IWindowManager>(builder);
        }
    }
}