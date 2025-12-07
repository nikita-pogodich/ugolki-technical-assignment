using Core.PresenterProvider;
using Core.WindowManager;
using Core.WindowViewProvider;
using Core.WorldObjectManager;
using Features.GameHUDWindow;
using Features.GameResultPopup;
using Features.MainMenu;
using Features.MainMenu.UgolkiRulesList;
using Features.MainMenu.UgolkiRulesListItem;
using Features.MessagePopup;
using Features.UgolkiLogic;
using VContainer;
using VContainer.Unity;

namespace Core.DI
{
    public class PresentersLifetimeScope : BaseLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterInitializableService<IWindowViewProvider, WindowViewProvider.WindowViewProvider>(builder);
            RegisterInitializableService<IPresenterProvider, PresenterProvider.PresenterProvider>(builder);
            RegisterInitializableService<IWindowManager, WindowManager.WindowManager>(builder);
            RegisterInitializableService<IWorldObjectManager, WorldObjectManager.WorldObjectManager>(builder);

            builder.Register<IGameHUDWindowPresenter, GameHUDWindowPresenter>(Lifetime.Transient);
            builder.Register<IGameResultPopupWindowPresenter, GameResultPopupWindowPresenter>(Lifetime.Transient);
            builder.Register<IMainMenuWindowPresenter, MainMenuWindowPresenter>(Lifetime.Transient);
            builder.Register<IUgolkiRulesListItemPresenter, UgolkiRulesListItemPresenter>(Lifetime.Transient);
            builder.Register<IUgolkiRulesListPresenter, UgolkiRulesListPresenter>(Lifetime.Transient);
            builder.Register<IMessagePopupWindowPresenter, MessagePopupWindowPresenter>(Lifetime.Transient);
            builder.Register<IUgolkiBoardPresenter, UgolkiBoardPresenter>(Lifetime.Transient);

            SetScopedServiceContainer<IPresenterProvider>(builder);

            builder.RegisterEntryPoint<GameBootstrapEntryPoint>();
        }
    }
}