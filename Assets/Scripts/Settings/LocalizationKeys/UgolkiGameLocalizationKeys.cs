using System;
using UnityEngine;

namespace Settings.LocalizationKeys
{
    [Serializable]
    public class UgolkiGameLocalizationKeys : IUgolkiGameLocalizationKeys
    {
        [field: SerializeField]
        public string WhiteMovesCount { get; private set; } = "white_moves_count";

        [field: SerializeField]
        public string BlackMovesCount { get; private set; } = "black_moves_count";

        [field: SerializeField]
        public string MovesCountValue { get; private set; } = "[moves]";

        [field: SerializeField]
        public string CurrentPlayer { get; private set; } = "current_player";

        [field: SerializeField]
        public string CurrentPlayerValue { get; private set; } = "[player]";

        [field: SerializeField]
        public string WhitePlayer { get; private set; } = "white_player";

        [field: SerializeField]
        public string BlackPlayer { get; private set; } = "black_player";
    }
}