using Core.MVP;
using R3;

namespace Features.MainMenu.UgolkiRulesListItem
{
    public interface IUgolkiRulesListItemModel : IModel
    {
        string RuleKey { get; }
        string TitleLocalizationKey { get; }
        ReadOnlyReactiveProperty<bool> IsSelected { get; }
        Observable<string> RuleSelected { get; }
        void SetRuleKey(string ruleKey);
        void SelectRule();
        void SetSelected(bool isSelected);
    }
}