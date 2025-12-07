using System;
using UnityEngine;

namespace Settings.LocalizationKeys
{
    [Serializable]
    public class LocalizationKeys : ILocalizationKeys
    {
        [SerializeField]
        private GameResultPopupLocalizationKeys _gameResultPopupLocalizationKeys;

        [SerializeField]
        private MainMenuLocalizationKeys _mainMenuLocalizationKeys;

        [SerializeField]
        private MessagePopupLocalizationKeys _messagePopupLocalizationKeys;

        [SerializeField]
        private UgolkiGameLocalizationKeys _ugolkiGameLocalizationKeys;

        public IGameResultPopupLocalizationKeys GameResultPopupLocalizationKeys => _gameResultPopupLocalizationKeys;
        public IMainMenuLocalizationKeys MainMenuLocalizationKeys => _mainMenuLocalizationKeys;
        public IMessagePopupLocalizationKeys MessagePopupLocalizationKeys => _messagePopupLocalizationKeys;
        public IUgolkiGameLocalizationKeys UgolkiGameLocalizationKeys => _ugolkiGameLocalizationKeys;
    }
}