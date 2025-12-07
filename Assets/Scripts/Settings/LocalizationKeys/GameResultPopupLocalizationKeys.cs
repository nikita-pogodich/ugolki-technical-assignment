using System;
using UnityEngine;

namespace Settings.LocalizationKeys
{
    [Serializable]
    public class GameResultPopupLocalizationKeys : IGameResultPopupLocalizationKeys
    {
        [field: SerializeField]
        public string GamerResultWhiteWins { get; private set; } = "gamer_result_white_wins";

        [field: SerializeField]
        public string GamerResultBlackWins { get; private set; } = "gamer_result_black_wins";
    }
}