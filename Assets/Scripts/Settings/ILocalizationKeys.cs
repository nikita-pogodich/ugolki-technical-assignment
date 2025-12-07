using Settings.LocalizationKeys;

namespace Settings
{
    public interface ILocalizationKeys
    {
        IGameResultPopupLocalizationKeys GameResultPopupLocalizationKeys { get; }
        IMainMenuLocalizationKeys MainMenuLocalizationKeys { get; }
        IMessagePopupLocalizationKeys MessagePopupLocalizationKeys { get; }
        IUgolkiGameLocalizationKeys UgolkiGameLocalizationKeys { get; }
    }
}