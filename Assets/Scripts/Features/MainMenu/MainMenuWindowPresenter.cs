using Core.PresenterProvider;
using Core.WindowManager;
using Cysharp.Threading.Tasks;
using Settings;
using UnityEngine;
using Features.MainMenu.UgolkiRulesList;
using Features.MainMenu.UgolkiRulesListItem;
using Features.UgolkiLogic;
using R3;
using VContainer;
using ViewInterfaces;

namespace Features.MainMenu
{
    public class MainMenuWindowPresenter :
        BaseWindowPresenter<IMainMenuWindowView, IMainMenuModel>,
        IMainMenuWindowPresenter
    {
        private readonly IWindowManager _windowManager;
        private readonly IUgolkiModel _ugolkiModel;
        private readonly ILocalSettings _localSettings;
        private readonly IPresenterProvider _presenterProvider;
        private readonly ILoadingScreenView _loadingScreenView;

        [Inject]
        public MainMenuWindowPresenter(
            IWindowManager windowManager,
            IUgolkiModel ugolkiModel,
            ILocalSettings localSettings,
            IPresenterProvider presenterProvider,
            ILoadingScreenView loadingScreenView)
        {
            _windowManager = windowManager;
            _ugolkiModel = ugolkiModel;
            _localSettings = localSettings;
            _presenterProvider = presenterProvider;
            _loadingScreenView = loadingScreenView;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            var ugolkiRulesListPresenter = _presenterProvider.Get<
                IUgolkiRulesListPresenter,
                IUgolkiRulesListView,
                IUgolkiRulesListModel>(
                View.UgolkiRulesListView,
                Model.UgolkiRulesListModel);

            AddChildPresenter(ugolkiRulesListPresenter);

            View.StartGame.Subscribe(OnStartGame).AddTo(ref disposableBuilder);
            View.ExitGame.Subscribe(OnExitGame).AddTo(ref disposableBuilder);
            Model.UgolkiRulesListModel.SelectedRule.Subscribe(OnRuleSelected).AddTo(ref disposableBuilder);
        }

        protected override void OnDeinit()
        {
            Model.Deinit();
        }

        protected override void OnShow()
        {
            OnRuleSelected(Model.UgolkiRulesListModel.SelectedRule.CurrentValue);
        }

        private void OnRuleSelected(IUgolkiRulesListItemModel ugolkiRulesListItemModel)
        {
            if (IsShown == false || ugolkiRulesListItemModel == null)
            {
                return;
            }

            _ugolkiModel.SetRule(ugolkiRulesListItemModel.RuleKey);
        }

        private void OnStartGame(Unit _)
        {
            _loadingScreenView.FadeIn(StartGame);
        }

        private void StartGame()
        {
            if (IsShown == false)
            {
                return;
            }

            _ugolkiModel.StartGame();
            SetShown(false);

            _windowManager.ShowWindowAsync<IGameHUDWindowView, IUgolkiModel>(_localSettings.ViewNames.GameHUD).Forget();
        }

        private void OnExitGame(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            Application.Quit();
        }
    }
}