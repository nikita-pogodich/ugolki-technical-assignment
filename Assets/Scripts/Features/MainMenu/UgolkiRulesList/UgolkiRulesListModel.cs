using System.Collections.Generic;
using Core.ModelProvider;
using Cysharp.Threading.Tasks;
using Features.MainMenu.UgolkiRulesListItem;
using Features.UgolkiLogic;
using R3;
using Settings;
using VContainer;

namespace Features.MainMenu.UgolkiRulesList
{
    public class UgolkiRulesListModel : BaseModel, IUgolkiRulesListModel
    {
        private readonly ILocalSettings _localSettings;
        private readonly IUgolkiModel _ugolkiModel;
        private readonly IModelProvider _modelProvider;

        private readonly Dictionary<string, IUgolkiRulesListItemModel> _ruleModelsByKey = new();
        private readonly ReactiveProperty<IUgolkiRulesListItemModel> _selectedRule = new();
        private readonly CompositeDisposable _reactiveCompositeDisposable = new();

        [Inject]
        public UgolkiRulesListModel(
            IModelProvider modelProvider,
            ILocalSettings localSettings,
            IUgolkiModel ugolkiModel) : base(modelProvider)
        {
            _localSettings = localSettings;
            _ugolkiModel = ugolkiModel;
            _modelProvider = modelProvider;
        }

        public IReadOnlyDictionary<string, IUgolkiRulesListItemModel> RuleModelsByKey => _ruleModelsByKey;
        public ReadOnlyReactiveProperty<IUgolkiRulesListItemModel> SelectedRule => _selectedRule;

        protected override async UniTask OnInit()
        {
            List<string> ruleKeys = _ugolkiModel.GetRules();

            foreach (string ruleKey in ruleKeys)
            {
                var ugolkiRulesListItemModel = await _modelProvider.GetAsync<IUgolkiRulesListItemModel>();
                ugolkiRulesListItemModel.SetRuleKey(ruleKey);
                ugolkiRulesListItemModel.RuleSelected.Subscribe(OnRuleSelected).AddTo(_reactiveCompositeDisposable);
                _ruleModelsByKey.Add(ruleKey, ugolkiRulesListItemModel);
            }

            OnRuleSelected(_localSettings.UgolkiRulesSettings.DefaultRule);
        }

        protected override void OnDeinit()
        {
            _ruleModelsByKey.Clear();
            _reactiveCompositeDisposable.Dispose();
        }

        private void OnRuleSelected(string ruleKey)
        {
            if (_ruleModelsByKey.TryGetValue(ruleKey, out IUgolkiRulesListItemModel rulesListItemModel) == false)
            {
                return;
            }

            foreach (IUgolkiRulesListItemModel ugolkiRulesListItemModel in _ruleModelsByKey.Values)
            {
                ugolkiRulesListItemModel.SetSelected(false);
            }

            rulesListItemModel.SetSelected(true);
            _selectedRule.Value = rulesListItemModel;
        }
    }
}