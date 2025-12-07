using System.Collections.Generic;
using System.Threading;
using Core.ResourcesManager;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Settings;
using Settings.LocalizationKeys;
using Tools;
using UnityEngine;
using VContainer;

namespace Core.SettingsHelper
{
    public class SettingsHelper : ISettingsHelper
    {
        private readonly Dictionary<string, string> _ugolkiRulesMap = new();
        private readonly ILocalSettings _localSettings;
        private readonly IResourcesManager _resourcesManager;

        public IReadOnlyDictionary<string, string> UgolkiRulesMap => _ugolkiRulesMap;

        public bool IsInitialized { get; private set; } = false;

        [Inject]
        public SettingsHelper(ILocalSettings localSettings, IResourcesManager resourcesManager)
        {
            _localSettings = localSettings;
            _resourcesManager = resourcesManager;
        }

        public async UniTask InitializeAsync(CancellationToken cancellation)
        {
            IUgolkiRulesSettings ugolkiRulesSettings = _localSettings.UgolkiRulesSettings;
            IMainMenuLocalizationKeys mainMenuLocalizationKeys =
                _localSettings.LocalizationKeys.MainMenuLocalizationKeys;

            _ugolkiRulesMap.Add(ugolkiRulesSettings.Rule1, mainMenuLocalizationKeys.UgolkiRule1);
            _ugolkiRulesMap.Add(ugolkiRulesSettings.Rule2, mainMenuLocalizationKeys.UgolkiRule2);
            _ugolkiRulesMap.Add(ugolkiRulesSettings.Rule3, mainMenuLocalizationKeys.UgolkiRule3);

            await UniTask.CompletedTask;

            IsInitialized = true;
        }

        public async UniTask<BoardCellType[,]> GetDefaultBoardPositionsAsync()
        {
            var defaultBoardPositionsTextAsset =
                await _resourcesManager.LoadAssetAsync<TextAsset>(_localSettings.ResourceNames.DefaultBoardPositions);

            string text = defaultBoardPositionsTextAsset.text;
            BoardCellType[,] defaultBoardPositions = await DeserializeJsonAsync<BoardCellType[,]>(text);
            return defaultBoardPositions;
        }

        public async UniTask<List<Coord>> GetWhiteWinConditionsAsync()
        {
            var whiteWinConditionsBoardTextAsset =
                _resourcesManager.LoadAsset<TextAsset>(_localSettings.ResourceNames.WhiteWinConditionsBoard);

            List<Coord> whiteWinConditions = await GetWinConditions(
                whiteWinConditionsBoardTextAsset,
                BoardCellType.White);

            return whiteWinConditions;
        }

        public async UniTask<List<Coord>> GetBlackWinConditionsAsync()
        {
            var blackWinConditionsBoardTextAsset =
                await _resourcesManager.LoadAssetAsync<TextAsset>(_localSettings.ResourceNames.BlackWinConditionsBoard);

            List<Coord> whiteWinConditions = await GetWinConditions(
                blackWinConditionsBoardTextAsset,
                BoardCellType.Black);

            return whiteWinConditions;
        }

        private async UniTask<List<Coord>> GetWinConditions(
            TextAsset winConditionsBoardTextAsset,
            BoardCellType boardCellType)
        {
            string text = winConditionsBoardTextAsset.text;
            BoardCellType[,] winConditionsBoard = await DeserializeJsonAsync<BoardCellType[,]>(text);

            List<Coord> winConditions = new();

            for (int i = 0; i < winConditionsBoard.GetLength(0); i++)
            {
                for (int j = 0; j < winConditionsBoard.GetLength(1); j++)
                {
                    if (winConditionsBoard[i, j] == boardCellType)
                    {
                        winConditions.Add(new Coord(i, j));
                    }
                }
            }

            return winConditions;
        }

        private async UniTask<TData> DeserializeJsonAsync<TData>(string jsonString) where TData : class
        {
            await UniTask.SwitchToThreadPool();
            var data = JsonConvert.DeserializeObject<TData>(jsonString);
            await UniTask.SwitchToMainThread();

            return data;
        }
    }
}