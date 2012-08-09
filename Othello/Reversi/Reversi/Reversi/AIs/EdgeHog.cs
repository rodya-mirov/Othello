using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversi.AIs
{
    public class EdgeHog : NPlyAI
    {
        public EdgeHog(int ply)
            : base(ply)
        {
        }

        protected override int evaluateBoard(int currentTurn, GameBoard board)
        {
            int whiteEdges = 0;
            int blackEdges = 0;

            for (int index = 0; index < 8; ++index)
            {
                if (board.cells[index, 0] == GameBoard.WHITE)
                    whiteEdges += 1;
                else if (board.cells[index, 0] == GameBoard.BLACK)
                    blackEdges += 1;

                if (board.cells[index, 7] == GameBoard.WHITE)
                    whiteEdges += 1;
                else if (board.cells[index, 7] == GameBoard.BLACK)
                    blackEdges += 1;

                if (board.cells[0, index] == GameBoard.WHITE)
                    whiteEdges += 1;
                else if (board.cells[0, index] == GameBoard.BLACK)
                    blackEdges += 1;

                if (board.cells[7, index] == GameBoard.WHITE)
                    whiteEdges += 1;
                else if (board.cells[7, index] == GameBoard.BLACK)
                    blackEdges += 1;
            }

            if (currentTurn == GameBoard.WHITE)
                return whiteEdges - blackEdges;
            else
                return blackEdges - whiteEdges;
        }
    }
}
