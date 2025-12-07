using Core.MVP;
using Features.MainMenu.UgolkiRulesList;

namespace Features.MainMenu
{
    public interface IMainMenuModel : IModel
    {
        IUgolkiRulesListModel UgolkiRulesListModel { get; }
        void Deinit();
    }
}