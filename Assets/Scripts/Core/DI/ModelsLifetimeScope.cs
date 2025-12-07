using Core.ModelProvider;
using Features.GameResultPopup;
using Features.MainMenu;
using Features.MainMenu.UgolkiRulesList;
using Features.MainMenu.UgolkiRulesListItem;
using Features.MessagePopup;
using VContainer;

namespace Core.DI
{
    public class ModelsLifetimeScope : BaseLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IUgolkiRulesListModel, UgolkiRulesListModel>(Lifetime.Transient);
            builder.Register<IUgolkiRulesListItemModel, UgolkiRulesListItemModel>(Lifetime.Transient);
            builder.Register<IMainMenuModel, MainMenuModel>(Lifetime.Transient);
            builder.Register<IGameResultPopupModel, GameResultPopupModel>(Lifetime.Transient);
            builder.Register<IMessagePopupModel, MessagePopupModel>(Lifetime.Transient);

            SetScopedServiceContainer<IModelProvider>(builder);
        }
    }
}