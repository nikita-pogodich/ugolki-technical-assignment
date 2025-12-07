using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class ViewNames : IViewNames
    {
        [field: SerializeField]
        public string MainMenu { get; private set; } = "MainMenuWindow";

        [field: SerializeField]
        public string GameHUD { get; private set; } = "UgolkiGameHUDWindow";

        [field: SerializeField]
        public string MessagePopup { get; private set; } = "MessagePopupWindow";

        [field: SerializeField]
        public string GameResultPopup { get; private set; } = "GameResultPopupWindow";

        [field: SerializeField]
        public string UgolkiRulesListItem { get; private set; } = "UgolkiRuleListItem";
        
        [field: SerializeField]
        public string UgolkiBoard { get; private set; } = "UgolkiBoard";
    }
}