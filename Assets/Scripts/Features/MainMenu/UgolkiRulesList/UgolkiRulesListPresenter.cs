using System.Collections.Generic;
using Core.ViewProvider;
using Core.PresenterProvider;
using Settings;
using Features.MainMenu.UgolkiRulesListItem;
using R3;
using VContainer;
using ViewInterfaces;

namespace Features.MainMenu.UgolkiRulesList
{
    public class UgolkiRulesListPresenter :
        BasePresenter<IUgolkiRulesListView, IUgolkiRulesListModel>,
        IUgolkiRulesListPresenter
    {
        private readonly IViewProvider _viewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly IPresenterProvider _presenterProvider;
        private readonly List<IUgolkiRulesListItemPresenter> _ugolkiRulePresenters = new();

        [Inject]
        public UgolkiRulesListPresenter(
            IViewProvider viewProvider,
            ILocalSettings localSettings,
            IPresenterProvider presenterProvider)
        {
            _viewProvider = viewProvider;
            _localSettings = localSettings;
            _presenterProvider = presenterProvider;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            CreateRulesList();
        }

        protected override void OnDeinit()
        {
            ClearRulesList();
        }

        private void CreateRulesList()
        {
            foreach (IUgolkiRulesListItemModel ugolkiRulesListItemModel in Model.RuleModelsByKey.Values)
            {
                IUgolkiRulesListItemView ugolkiRulesListItemView =
                    _viewProvider.Get<IUgolkiRulesListItemView>(_localSettings.ViewNames.UgolkiRulesListItem);

                var rulesListItemPresenter = _presenterProvider.Get<
                    IUgolkiRulesListItemPresenter,
                    IUgolkiRulesListItemView,
                    IUgolkiRulesListItemModel>(
                    ugolkiRulesListItemView,
                    ugolkiRulesListItemModel);

                _ugolkiRulePresenters.Add(rulesListItemPresenter);
                AddChildPresenter(rulesListItemPresenter);

                View.AddItem(ugolkiRulesListItemView);
            }
        }

        private void ClearRulesList()
        {
            for (int i = 0; i < _ugolkiRulePresenters.Count; i++)
            {
                IUgolkiRulesListItemPresenter ugolkiRulesListItemPresenter = _ugolkiRulePresenters[i];
                RemoveChildPresenter(ugolkiRulesListItemPresenter);

                IUgolkiRulesListItemView ugolkiRulesListItemView = ugolkiRulesListItemPresenter.View;
                ugolkiRulesListItemView.Deinit();

                _viewProvider.Release(_localSettings.ViewNames.UgolkiRulesListItem, ugolkiRulesListItemView);
            }

            _ugolkiRulePresenters.Clear();
        }
    }
}