namespace Settings
{
    public interface ILocalSettings
    {
        IGameSettings GameSettings { get; }
        IUgolkiRulesSettings UgolkiRulesSettings { get; }
        IViewNames ViewNames { get; }
        IResourceNames ResourceNames { get; }
        ILocalizationKeys LocalizationKeys { get; }
    }
}