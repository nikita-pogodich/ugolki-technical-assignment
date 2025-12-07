using R3;
using UnityEngine;
using UnityEngine.UI;
using ViewInterfaces;

namespace Views.MainMenu
{
    public class MainMenuWindowView : BaseWindowView, IMainMenuWindowView
    {
        [SerializeField]
        private UgolkiRulesListView _ugolkiRulesList;

        [SerializeField]
        private Button _startGameButton;

        [SerializeField]
        private Button _exitButton;

        private readonly ReactiveCommand _startGame = new();
        private readonly ReactiveCommand _exitGame = new();

        public Observable<Unit> StartGame => _startGame;
        public Observable<Unit> ExitGame => _exitGame;

        public IUgolkiRulesListView UgolkiRulesListView => _ugolkiRulesList;

        public override void SetShown(bool isShown)
        {
            base.SetShown(isShown);
            SetCanvasEnabled(isShown);
        }

        private void Start()
        {
            _startGameButton.onClick.AddListener(OnStartGame);
            _exitButton.onClick.AddListener(OnExitGame);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(OnStartGame);
            _exitButton.onClick.RemoveListener(OnExitGame);
        }

        private void OnStartGame()
        {
            _startGame.Execute(Unit.Default);
        }

        private void OnExitGame()
        {
            _exitGame.Execute(Unit.Default);
        }
    }
}