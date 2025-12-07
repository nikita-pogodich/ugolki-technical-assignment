using System.Collections.Generic;
using R3;

namespace Core.LocalizationManager
{
    public interface ILocalizationManager
    {
        ReadOnlyReactiveProperty<LanguageInfo> CurrentLanguage { get; }

        string GetText(string key);
        string GetText(string key, string keyToReplace, string valueToReplace);

        void SetLocale(string key);
        IEnumerable<LanguageInfo> GetLocales();
    }
}