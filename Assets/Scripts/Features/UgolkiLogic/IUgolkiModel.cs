using System.Collections.Generic;
using Core.MVP;
using R3;
using Settings;
using Tools;

namespace Features.UgolkiLogic
{
    public interface IUgolkiModel : IModel
    {
        BoardCellType[,] Board { get; }
        Observable<List<Coord>> PieceMoved { get; }
        Observable<Coord> PieceSelected { get; }
        Observable<Coord> PieceDeselected { get; }
        Observable<Unit> GameStarted { get; }
        Observable<Unit> GameEnded { get; }
        Observable<Player> GameWon { get; }
        Observable<string> WrongMoveSelected { get; }
        ReadOnlyReactiveProperty<int> WhiteMovesAmount { get; }
        ReadOnlyReactiveProperty<int> BlackMovesAmount { get; }
        ReadOnlyReactiveProperty<Player> CurrentPlayer { get; }
        List<string> GetRules();
        void SetRule(string rule);
        void StartGame();
        void EndGame();
        void RestartGame();
        void TrySelectCell(Coord cell);
    }
}