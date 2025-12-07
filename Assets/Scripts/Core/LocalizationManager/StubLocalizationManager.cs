using System.Collections.Generic;
using Core.Logger;
using R3;
using VContainer;

namespace Core.LocalizationManager
{
    public class StubLocalizationManager : ILocalizationManager
    {
        private readonly IDualLogger _dualLogger;
        private readonly ReactiveProperty<LanguageInfo> _currentLanguage = new();
        private readonly Dictionary<string, string> _languageTerms;
        private readonly Dictionary<string, LanguageInfo> _languageInfosByCode = new();

        public ReadOnlyReactiveProperty<LanguageInfo> CurrentLanguage => _currentLanguage;

        [Inject]
        public StubLocalizationManager(IDualLogger dualLogger)
        {
            _dualLogger = dualLogger;
            var language = new LanguageInfo {Code = "en", DisplayName = "English", Icon = "English"};
            _languageInfosByCode.Add(language.Code, language);
            _currentLanguage.Value = language;

            _languageTerms = new Dictionary<string, string>
            {
                {"main_menu_game_title", "Ugolki"},
                {"start_game_button", "Start Game"},
                {"ugolki_rule_1", "Pieces can jump over another diagonally"},
                {"ugolki_rule_2", "Pieces can jump vertically and horizontally"},
                {"ugolki_rule_3", "Pieces cannot jump, but only take one step in either direction"},
                {"white_moves_count", "White: [moves]"},
                {"black_moves_count", "Black: [moves]"},
                {"current_player", "Current player:<br>[player]"},
                {"white_player", "White"},
                {"black_player", "Black"},
                {"not_your_move_message", "Not your move"},
                {"select_piece_message", "Select piece"},
                {"move_unreachable_message", "Move unreachable"},
                {"gamer_result_white_wins", "White player wins"},
                {"gamer_result_black_wins", "Black player wins"}
            };
        }

        public string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _dualLogger.Mandatory.LogError("Localization key is empty");
                return string.Empty;
            }

            bool hasKey = _languageTerms.TryGetValue(key, out string result);
            if (hasKey == false)
            {
                _dualLogger.Mandatory.LogError($"Localization key not found {key}");
                result = key;
            }

            return result;
        }

        public string GetText(string key, string keyToReplace, string valueToReplace)
        {
            string sourceString = (this as ILocalizationManager).GetText(key);
            string result = ReplaceWithLocalizedString(sourceString, keyToReplace, valueToReplace);
            return result;
        }

        public void SetLocale(string key)
        {
            if (_languageInfosByCode.TryGetValue(key, out LanguageInfo languageInfo))
            {
                _currentLanguage.Value = languageInfo;
            }
        }

        public IEnumerable<LanguageInfo> GetLocales()
        {
            return _languageInfosByCode.Values;
        }

        private string ReplaceWithLocalizedString(string sourceString, string keyToReplace, string value)
        {
            string result = sourceString;
            if (string.IsNullOrEmpty(sourceString) == false)
            {
                result = result.Replace(keyToReplace, value);
            }

            return result;
        }
    }
}