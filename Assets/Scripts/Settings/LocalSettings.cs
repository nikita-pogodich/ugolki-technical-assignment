using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(
        fileName = "LocalSettings",
        menuName = "Config/LocalSettings",
        order = 0
    )]
    public class LocalSettings : ScriptableObject, ILocalSettings
    {
        [SerializeField]
        private GameSettings _gameSettings;

        [SerializeField]
        private UgolkiRulesSettings _ugolkiRulesSettings;

        [SerializeField]
        private ViewNames _viewNames;

        [SerializeField]
        private ResourceNames _resourceNames;

        [SerializeField]
        private LocalizationKeys.LocalizationKeys _localizationKeys;

        public IGameSettings GameSettings => _gameSettings;
        public IUgolkiRulesSettings UgolkiRulesSettings => _ugolkiRulesSettings;
        public IViewNames ViewNames => _viewNames;
        public IResourceNames ResourceNames => _resourceNames;
        public ILocalizationKeys LocalizationKeys => _localizationKeys;
    }
}