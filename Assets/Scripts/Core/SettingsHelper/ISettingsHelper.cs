using System.Collections.Generic;
using Core.Services;
using Cysharp.Threading.Tasks;
using Settings;
using Tools;

namespace Core.SettingsHelper
{
    public interface ISettingsHelper : IInitializableService
    {
        IReadOnlyDictionary<string, string> UgolkiRulesMap { get; }
        UniTask<BoardCellType[,]> GetDefaultBoardPositionsAsync();
        UniTask<List<Coord>> GetWhiteWinConditionsAsync();
        UniTask<List<Coord>> GetBlackWinConditionsAsync();
    }
}