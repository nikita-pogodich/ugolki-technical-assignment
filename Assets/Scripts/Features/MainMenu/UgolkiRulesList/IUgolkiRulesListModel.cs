using System.Collections.Generic;
using Core.MVP;
using Features.MainMenu.UgolkiRulesListItem;
using R3;

namespace Features.MainMenu.UgolkiRulesList
{
    public interface IUgolkiRulesListModel : IModel
    {
        IReadOnlyDictionary<string, IUgolkiRulesListItemModel> RuleModelsByKey { get; }
        ReadOnlyReactiveProperty<IUgolkiRulesListItemModel> SelectedRule { get; }
        void Deinit();
    }
}