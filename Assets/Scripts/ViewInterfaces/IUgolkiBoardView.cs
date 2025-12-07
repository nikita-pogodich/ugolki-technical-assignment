using System.Collections.Generic;
using Core.MVP;
using Cysharp.Threading.Tasks;
using R3;
using Settings;
using Tools;

namespace ViewInterfaces
{
    public interface IUgolkiBoardView : IWorldObjectView
    {
        Observable<Coord> TrySelectCell { get; }
        UniTask StartGame(BoardCellType[,] board);
        void PauseGame();
        void EndGame();
        void SelectPiece(Coord coord);
        void DeselectPiece(Coord coord);
        void MovePiece(List<Coord> moves);
    }
}