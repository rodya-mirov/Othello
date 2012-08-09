using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reversi.AIs
{
    public abstract class NPlyAI : AI
    {
        private int n;

        public NPlyAI(int n)
            : base()
        {
            this.n = n;
        }

        public override void getDesiredMove()
        {
            Point bestMove = new Point(-1, -1);
            int bestScore = int.MinValue;

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    if (!storedBoard.canPlayAtPosition(x, y))
                        continue;

                    GameBoard result = getResultOfNPlay(storedBoard.makeMove(x, y), n);
                    int score = evaluateBoard(storedBoard.currentTurn, result);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new Point(x, y);
                    }
                }
            }

            desiredMove = bestMove;
        }

        private GameBoard getResultOfNPlay(GameBoard board, int ply)
        {
            if (ply == 0)
                return baseResult(board);

            int bestScore = int.MinValue;
            GameBoard output = board;

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    if (!board.canPlayAtPosition(x, y))
                        continue;

                    GameBoard playBoard = board.makeMove(x, y);
                    GameBoard result = getResultOfNPlay(playBoard, ply - 1);

                    int score = evaluateBoard(board.currentTurn, result);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        output = result;
                    }
                }
            }

            return output;
        }

        protected abstract int evaluateBoard(int currentTurn, GameBoard result);

        private GameBoard baseResult(GameBoard board)
        {
            int bestScore = int.MinValue;
            GameBoard output = board;

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    if (!board.canPlayAtPosition(x, y))
                        continue;

                    GameBoard playBoard = board.makeMove(x, y);
                    int score = evaluateBoard(board.currentTurn, playBoard);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        output = playBoard;
                    }
                }
            }

            return output;
        }
    }
}
