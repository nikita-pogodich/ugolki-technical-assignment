using R3;
using ViewInterfaces;

namespace Features.GameResultPopup
{
    public class GameResultPopupWindowPresenter :
        BaseWindowPresenter<IGameResultPopupWindowView, IGameResultPopupModel>,
        IGameResultPopupWindowPresenter
    {
        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            Model.GameResultLocalizationKey.Subscribe(OnGameResultLocalizationKeyChanged).AddTo(ref disposableBuilder);
            View.BackToMenu.Subscribe(OnBackToMenu).AddTo(ref disposableBuilder);
            View.RestartGame.Subscribe(OnRestartGame).AddTo(ref disposableBuilder);
        }

        protected override void OnShow()
        {
            View.GameResultLocalizationKey.Value = Model.GameResultLocalizationKey.CurrentValue;
        }

        private void OnGameResultLocalizationKeyChanged(string gameResultLocalizationKey)
        {
            if (IsShown == false)
            {
                return;
            }

            View.GameResultLocalizationKey.Value = gameResultLocalizationKey;
        }

        private void OnRestartGame(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            Model.OnRestart();
            SetShown(false);
        }

        private void OnBackToMenu(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            Model.OnBackToMenu();
            SetShown(false);
        }
    }
}