using System;
using Core.MVP;
using Features.UgolkiLogic;
using R3;

namespace Features.GameResultPopup
{
    public interface IGameResultPopupModel : IModel
    {
        ReadOnlyReactiveProperty<string> GameResultLocalizationKey { get; } 
        void UpdateModel(Player player, Action restart, Action exit);
        void OnRestart();
        void OnBackToMenu();
    }
}