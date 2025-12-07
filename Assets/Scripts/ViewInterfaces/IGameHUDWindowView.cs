using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IGameHUDWindowView : IWindowView
    {
        Observable<Unit> Back { get; }
        void SetWhiteMovesAmount(int count);
        void SetBlackMovesAmount(int count);
        void ChangeCurrentPlayer(string currentPlayer);
    }
}