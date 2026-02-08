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

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _startGameButton.OnClickAsObservable().Subscribe(OnStartGame).AddTo(ref disposableBuilder);
            _exitButton.OnClickAsObservable().Subscribe(OnExitGame).AddTo(ref disposableBuilder);
        }

        private void OnStartGame(Unit _)
        {
            _startGame.Execute(Unit.Default);
        }

        private void OnExitGame(Unit _)
        {
            _exitGame.Execute(Unit.Default);
        }
    }
}