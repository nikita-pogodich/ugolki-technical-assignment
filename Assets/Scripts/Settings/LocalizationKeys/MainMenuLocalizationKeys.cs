using System;
using UnityEngine;

namespace Settings.LocalizationKeys
{
    [Serializable]
    public class MainMenuLocalizationKeys : IMainMenuLocalizationKeys
    {
        [field: SerializeField]
        public string StartGameButton { get; private set; } = "start_game_button";

        [field: SerializeField]
        public string UgolkiRule1 { get; private set; } = "ugolki_rule_1";

        [field: SerializeField]
        public string UgolkiRule2 { get; private set; } = "ugolki_rule_2";

        [field: SerializeField]
        public string UgolkiRule3 { get; private set; } = "ugolki_rule_3";
    }
}