using Core.LocalizationManager;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ViewInterfaces;

namespace Views.GameResultPopup
{
    public class GameResultPopupWindowView : BaseWindowView, IGameResultPopupWindowView
    {
        [SerializeField]
        private TextMeshProUGUI _gameResult;

        [SerializeField]
        private Button _backToMenuButton;

        [SerializeField]
        private Button _restartGameButton;

        [SerializeField]
        private WindowFadeAnimationView _fadeAnimation;

        private ILocalizationManager _localizationManager;

        private readonly ReactiveCommand _backToMenu = new();
        private readonly ReactiveCommand _restartGame = new();

        public Observable<Unit> BackToMenu => _backToMenu;
        public Observable<Unit> RestartGame => _restartGame;
        public ReactiveProperty<string> GameResultLocalizationKey { get; } = new();

        [Inject]
        public void InjectDependencies(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            GameResultLocalizationKey.Subscribe(SetGameResult).AddTo(ref disposableBuilder);
            _backToMenuButton.OnClickAsObservable().Subscribe(OnBackToMenu).AddTo(ref disposableBuilder);
            _restartGameButton.OnClickAsObservable().Subscribe(OnRestartGame).AddTo(ref disposableBuilder);

            _fadeAnimation.Init();
        }

        public override void SetShown(bool isShown)
        {
            base.SetShown(isShown);
            _fadeAnimation.SetShown(isShown);
        }

        private void SetGameResult(string gameResultLocalizationKey)
        {
            if (string.IsNullOrEmpty(gameResultLocalizationKey))
            {
                return;
            }

            var gameResult = _localizationManager.GetText(gameResultLocalizationKey);
            _gameResult.text = gameResult;
        }

        private void OnBackToMenu(Unit _)
        {
            _backToMenu.Execute(Unit.Default);
        }

        private void OnRestartGame(Unit _)
        {
            _restartGame.Execute(Unit.Default);
        }
    }
}