using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversi.AIs
{
    public class CornerAI : NPlyAI
    {
        private const int cornerWeight = 3;
        private const int cornerAdjacentWeight = -1;
        private const int edgeWeight = 1;

        public CornerAI(int ply)
            : base(ply)
        {
        }

        protected override int evaluateBoard(int currentTurn, GameBoard board)
        {
            int whiteScore = 0;
            int blackScore = 0;

            for (int index = 2; index < 6; ++index)
            {
                whiteScore += match(GameBoard.WHITE, board.cells[index, 0]) * edgeWeight;
                blackScore += match(GameBoard.BLACK, board.cells[index, 0]) * edgeWeight;

                whiteScore += match(GameBoard.WHITE, board.cells[index, 7]) * edgeWeight;
                blackScore += match(GameBoard.BLACK, board.cells[index, 7]) * edgeWeight;

                whiteScore += match(GameBoard.WHITE, board.cells[0, index]) * edgeWeight;
                blackScore += match(GameBoard.BLACK, board.cells[0, index]) * edgeWeight;

                whiteScore += match(GameBoard.WHITE, board.cells[7, index]) * edgeWeight;
                blackScore += match(GameBoard.BLACK, board.cells[7, index]) * edgeWeight;
            }

            if (board.cells[0, 0] == GameBoard.UNOCCUPIED)
            {
                whiteScore += match(GameBoard.WHITE, board.cells[1, 0]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[1, 0]) * cornerAdjacentWeight;

                whiteScore += match(GameBoard.WHITE, board.cells[0, 1]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[0, 1]) * cornerAdjacentWeight;
            }
            else
            {
                whiteScore += match(GameBoard.WHITE, board.cells[0, 0]) * cornerWeight;
                blackScore += match(GameBoard.BLACK, board.cells[0, 0]) * cornerWeight;
            }

            if (board.cells[7, 0] == GameBoard.UNOCCUPIED)
            {
                whiteScore += match(GameBoard.WHITE, board.cells[6, 0]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[6, 0]) * cornerAdjacentWeight;

                whiteScore += match(GameBoard.WHITE, board.cells[7, 1]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[7, 1]) * cornerAdjacentWeight;
            }
            else
            {
                whiteScore += match(GameBoard.WHITE, board.cells[7, 0]) * cornerWeight;
                blackScore += match(GameBoard.BLACK, board.cells[7, 0]) * cornerWeight;
            }

            if (board.cells[0, 7] == GameBoard.UNOCCUPIED)
            {
                whiteScore += match(GameBoard.WHITE, board.cells[1, 7]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[1, 7]) * cornerAdjacentWeight;

                whiteScore += match(GameBoard.WHITE, board.cells[0, 6]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[0, 6]) * cornerAdjacentWeight;
            }
            else
            {
                whiteScore += match(GameBoard.WHITE, board.cells[0, 7]) * cornerWeight;
                blackScore += match(GameBoard.BLACK, board.cells[0, 7]) * cornerWeight;
            }

            if (board.cells[7, 7] == GameBoard.UNOCCUPIED)
            {
                whiteScore += match(GameBoard.WHITE, board.cells[6, 7]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[6, 7]) * cornerAdjacentWeight;

                whiteScore += match(GameBoard.WHITE, board.cells[7, 6]) * cornerAdjacentWeight;
                blackScore += match(GameBoard.BLACK, board.cells[7, 6]) * cornerAdjacentWeight;
            }
            else
            {
                whiteScore += match(GameBoard.WHITE, board.cells[7, 7]) * cornerWeight;
                blackScore += match(GameBoard.BLACK, board.cells[7, 7]) * cornerWeight;
            }

            if (currentTurn == GameBoard.WHITE)
                return whiteScore - blackScore;
            else
                return blackScore - whiteScore;
        }
    }
}
