using System.Collections.Generic;
using Settings;
using Tools;

namespace Features.UgolkiLogic.UgolkiRules
{
    public class CannotJumpRule : BaseUgolkiRule
    {
        public CannotJumpRule(int boardSize) : base(boardSize)
        {
        }

        public override void TryAddAvailableMoves(
            BoardCellType[,] board,
            Coord fromCell,
            Dictionary<int, Node<Coord>> graph,
            Queue<Coord> toCheck,
            List<Coord> canJump)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    TryAddAvailableMove(board, fromCell, i, j, canJump);
                }
            }

            FillGraphMoves(graph, canJump);
        }
    }
}