using System;
using Core.ModelProvider;
using Features.UgolkiLogic;
using R3;
using Settings;
using VContainer;

namespace Features.GameResultPopup
{
    public class GameResultPopupModel : BaseModel, IGameResultPopupModel
    {
        private readonly ILocalSettings _localSettings;
        private readonly ReactiveProperty<string> _gameResultLocalizationKey = new();

        private event Action Restart;
        private event Action BackToMenu;

        public ReadOnlyReactiveProperty<string> GameResultLocalizationKey => _gameResultLocalizationKey;

        [Inject]
        public GameResultPopupModel(IModelProvider modelProvider, ILocalSettings localSettings) : base(modelProvider)
        {
            _localSettings = localSettings;
        }

        public void UpdateModel(Player player, Action restart, Action exit)
        {
            string resultKey;
            if (player == Player.White)
            {
                resultKey = _localSettings.LocalizationKeys.GameResultPopupLocalizationKeys.GamerResultWhiteWins;
            }
            else
            {
                resultKey = _localSettings.LocalizationKeys.GameResultPopupLocalizationKeys.GamerResultBlackWins;
            }

            _gameResultLocalizationKey.Value = resultKey;
            Restart = restart;
            BackToMenu = exit;
        }

        public void OnRestart()
        {
            Restart?.Invoke();
        }

        public void OnBackToMenu()
        {
            BackToMenu?.Invoke();
        }
    }
}