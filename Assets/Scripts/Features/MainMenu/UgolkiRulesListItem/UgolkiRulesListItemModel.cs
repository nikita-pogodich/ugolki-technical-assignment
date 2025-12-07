using Core.ModelProvider;
using Core.SettingsHelper;
using R3;
using VContainer;

namespace Features.MainMenu.UgolkiRulesListItem
{
    public class UgolkiRulesListItemModel : BaseModel, IUgolkiRulesListItemModel
    {
        private readonly ISettingsHelper _settingsHelper;
        private readonly ReactiveProperty<bool> _isSelected = new();

        private readonly ReactiveCommand<string> _ruleSelected = new();

        public string RuleKey { get; private set; }
        public string TitleLocalizationKey { get; private set; } = string.Empty;
        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        public Observable<string> RuleSelected => _ruleSelected;

        [Inject]
        public UgolkiRulesListItemModel(
            IModelProvider modelProvider,
            ISettingsHelper settingsHelper) : base(modelProvider)
        {
            _settingsHelper = settingsHelper;
        }

        public void SetRuleKey(string ruleKey)
        {
            RuleKey = ruleKey;

            RuleKey = ruleKey;

            if (_settingsHelper.UgolkiRulesMap.TryGetValue(
                    RuleKey,
                    out string titleLocalizationKey))
            {
                TitleLocalizationKey = titleLocalizationKey;
            }
        }

        public void SelectRule()
        {
            _ruleSelected.Execute(RuleKey);
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected.Value = isSelected;
        }
    }
}