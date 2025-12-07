using System.Collections.Generic;
using Settings;
using Tools;

namespace Features.UgolkiLogic.UgolkiRules
{
    public class CanJumpOrthogonallyRule : BaseUgolkiRule
    {
        public CanJumpOrthogonallyRule(int boardSize) : base(boardSize)
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
                    if (i == 0 && j == 0 ||
                        i == -1 && j == -1 ||
                        i == 1 && j == 1 ||
                        i == -1 && j == 1 ||
                        i == 1 && j == -1)
                    {
                        continue;
                    }

                    TryAddAvailableMove(board, fromCell, i, j, canJump);
                }
            }

            FillGraphMoves(graph, canJump);

            while (toCheck.Count > 0)
            {
                Coord currentFrom = toCheck.Dequeue();

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0 ||
                            i == -1 && j == -1 ||
                            i == 1 && j == 1 ||
                            i == -1 && j == 1 ||
                            i == 1 && j == -1)
                        {
                            continue;
                        }

                        TryAddAvailableJump(board, currentFrom, i, j, graph, canJump, toCheck);
                    }
                }
            }
        }
    }
}