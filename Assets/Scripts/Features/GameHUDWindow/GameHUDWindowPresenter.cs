using Core.LocalizationManager;
using Core.WindowManager;
using Cysharp.Threading.Tasks;
using Features.MainMenu;
using Features.UgolkiLogic;
using R3;
using Settings;
using Settings.LocalizationKeys;
using VContainer;
using ViewInterfaces;

namespace Features.GameHUDWindow
{
    public class GameHUDWindowPresenter :
        BaseWindowPresenter<IGameHUDWindowView, IUgolkiModel>,
        IGameHUDWindowPresenter
    {
        private readonly IWindowManager _windowManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ILocalSettings _localSettings;

        [Inject]
        public GameHUDWindowPresenter(
            IWindowManager windowManager,
            ILocalizationManager localizationManager,
            ILocalSettings localSettings)
        {
            _windowManager = windowManager;
            _localizationManager = localizationManager;
            _localSettings = localSettings;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            Model.WhiteMovesAmount.Subscribe(OnWhiteMovesAmountChanged).AddTo(ref disposableBuilder);
            Model.BlackMovesAmount.Subscribe(OnBlackMovesAmountChanged).AddTo(ref disposableBuilder);
            Model.CurrentPlayer.Subscribe(OnCurrentPlayerChanged).AddTo(ref disposableBuilder);

            View.Back.Subscribe(OnBack).AddTo(ref disposableBuilder);

            OnWhiteMovesAmountChanged(Model.WhiteMovesAmount.CurrentValue);
            OnWhiteMovesAmountChanged(Model.BlackMovesAmount.CurrentValue);
        }

        private void OnCurrentPlayerChanged(Player player)
        {
            string resultPlayerName;
            IUgolkiGameLocalizationKeys localizationKeys = _localSettings.LocalizationKeys.UgolkiGameLocalizationKeys;
            if (player == Player.White)
            {
                resultPlayerName = _localizationManager.GetText(localizationKeys.WhitePlayer);
            }
            else
            {
                resultPlayerName = _localizationManager.GetText(localizationKeys.BlackPlayer);
            }

            string currentPlayerText = _localizationManager.GetText(
                key: localizationKeys.CurrentPlayer,
                keyToReplace: localizationKeys.CurrentPlayerValue,
                valueToReplace: resultPlayerName);

            View.ChangeCurrentPlayer(currentPlayerText);
        }

        private void OnBack(Unit _)
        {
            Model.EndGame();
            SetShown(false);

            _windowManager.ShowWindowAsync<IMainMenuWindowView, IMainMenuModel>(
                _localSettings.ViewNames.MainMenu).Forget();
        }

        private void OnBlackMovesAmountChanged(int movesAmount)
        {
            View.SetBlackMovesAmount(movesAmount);
        }

        private void OnWhiteMovesAmountChanged(int movesAmount)
        {
            View.SetWhiteMovesAmount(movesAmount);
        }
    }
}