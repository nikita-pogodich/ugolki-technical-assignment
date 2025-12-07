using System.Collections.Generic;
using Core.WindowManager;
using Cysharp.Threading.Tasks;
using Features.GameResultPopup;
using Features.MainMenu;
using Features.MessagePopup;
using R3;
using Settings;
using Tools;
using VContainer;
using ViewInterfaces;

namespace Features.UgolkiLogic
{
    public class UgolkiBoardPresenter : BasePresenter<IUgolkiBoardView, IUgolkiModel>, IUgolkiBoardPresenter
    {
        private readonly IWindowManager _windowManager;
        private readonly ILocalSettings _localSettings;
        private readonly ILoadingScreenView _loadingScreenView;

        [Inject]
        public UgolkiBoardPresenter(
            IWindowManager windowManager,
            ILocalSettings localSettings,
            ILoadingScreenView loadingScreenView)
        {
            _windowManager = windowManager;
            _localSettings = localSettings;
            _loadingScreenView = loadingScreenView;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            Model.PieceMoved.Subscribe(OnPieceMoved).AddTo(ref disposableBuilder);
            Model.PieceSelected.Subscribe(OnPieceSelected).AddTo(ref disposableBuilder);
            Model.PieceDeselected.Subscribe(OnPieceDeselected).AddTo(ref disposableBuilder);
            Model.GameStarted.Subscribe(OnGameStarted).AddTo(ref disposableBuilder);
            Model.GameEnded.Subscribe(OnGameEnded).AddTo(ref disposableBuilder);
            Model.GameWon.Subscribe(OnGameWon).AddTo(ref disposableBuilder);
            Model.WrongMoveSelected.Subscribe(OnWrongMoveSelected).AddTo(ref disposableBuilder);
            View.TrySelectCell.Subscribe(OnTrySelectCell).AddTo(ref disposableBuilder);
        }

        private void OnRestart()
        {
            Model.RestartGame();
        }

        private void OnBackToMenu()
        {
            Model.EndGame();

            _windowManager.ShowWindowAsync<IMainMenuWindowView, IMainMenuModel>(
                _localSettings.ViewNames.MainMenu).Forget();

            SetShown(false);
        }

        private async void OnGameStarted(Unit _)
        {
            await View.StartGame(Model.Board);
            _loadingScreenView.FadeOut();
            SetShown(true);
        }

        private void OnGameEnded(Unit _)
        {
            View.EndGame();
            SetShown(false);
        }

        private void OnPieceMoved(List<Coord> moves)
        {
            View.MovePiece(moves);
        }

        private void OnPieceSelected(Coord cell)
        {
            View.SelectPiece(cell);
        }

        private void OnPieceDeselected(Coord cell)
        {
            View.DeselectPiece(cell);
        }

        private void OnWrongMoveSelected(string wrongMoveLocalizationKey)
        {
            _windowManager.ShowWindowAsync<IMessagePopupView, IMessagePopupModel>(
                _localSettings.ViewNames.MessagePopup,
                beforeShow: UpdateMessagePopupModel,
                alreadyShown: UpdateMessagePopupModel).Forget();

            return;

            void UpdateMessagePopupModel(IMessagePopupModel messagePopupModel)
            {
                messagePopupModel.MessageLocalizationKey.Value = wrongMoveLocalizationKey;
            }
        }

        private void OnGameWon(Player player)
        {
            _windowManager.ShowWindowAsync<IGameResultPopupWindowView, IGameResultPopupModel>(
                _localSettings.ViewNames.GameResultPopup,
                beforeShow: OnBeforeGameResultPopupShow).Forget();

            View.PauseGame();
            return;

            void OnBeforeGameResultPopupShow(IGameResultPopupModel gameResultPopupModel)
            {
                gameResultPopupModel.UpdateModel(player, OnRestart, OnBackToMenu);
            }
        }

        private void OnTrySelectCell(Coord cell)
        {
            Model.TrySelectCell(cell);
        }
    }
}