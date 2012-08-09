using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Reversi
{
    public class GameBoard
    {
        public int[,] cells;

        public const int UNOCCUPIED = 0;
        public const int BLACK = 1;
        public const int WHITE = 2;

        public int currentTurn;

        public int whiteScore;
        public int blackScore;
        public int unoccupiedCells;

        public bool gameover;

        public GameBoard()
        {
            gameover = false;

            cells = new int[8, 8];

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    cells[x, y] = UNOCCUPIED;
                }
            }

            cells[3, 3] = WHITE;
            cells[4, 4] = WHITE;
            cells[3, 4] = BLACK;
            cells[4, 3] = BLACK;

            currentTurn = WHITE;
        }

        private GameBoard(GameBoard original)
        {
            gameover = original.gameover;
            cells = new int[8, 8];

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    cells[x, y] = original.cells[x, y];
                }
            }

            currentTurn = original.currentTurn;
            whiteScore = original.whiteScore;
            blackScore = original.blackScore;
        }

        public bool canPlayAtPosition(int x, int y)
        {
            if (!validPosition(x, y) || cells[x, y] != UNOCCUPIED)
                return false;

            int opposition = (currentTurn == WHITE) ? BLACK : WHITE;

            int a, b;
            int localCaptures = 0;

            //UL, U, UR, L, R, DL, D, DR
            int[] xDir = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] yDir = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int dir = 0; dir < 8; ++dir)
            {
                localCaptures = 0;

                int xvel = xDir[dir];
                int yvel = yDir[dir];

                a = x + xvel;
                b = y + yvel;

                while (validPosition(a, b) && cells[a, b] == opposition)
                {
                    localCaptures += 1;

                    a += xvel;
                    b += yvel;
                }

                if (validPosition(a, b) && cells[a, b] == currentTurn && localCaptures > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool validPosition(int a, int b)
        {
            return a >= 0 && a < 8 && b >= 0 && b < 8;
        }

        public void switchTurns()
        {
            currentTurn = (currentTurn == WHITE ? BLACK : WHITE);
        }

        public GameBoard makeMove(int x, int y)
        {
            GameBoard output = new GameBoard(this);

            output.playAtPosition(x, y);

            return output;
        }

        /// <summary>
        /// Note: this assumes that it's actually possible to play at the given position,
        /// and will not check if this is valid.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void playAtPosition(int x, int y)
        {
            cells[x, y] = currentTurn;

            int opposition = (currentTurn == WHITE) ? BLACK : WHITE;

            int a, b;

            //UL, U, UR, L, R, DL, D, DR
            int[] xDir = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] yDir = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int dir = 0; dir < 8; ++dir)
            {
                int xvel = xDir[dir];
                int yvel = yDir[dir];

                a = x + xvel;
                b = y + yvel;

                bool foundFriendlyCombo = false;

                while (validPosition(a, b) && cells[a, b] == opposition)
                {
                    a += xvel;
                    b += yvel;
                }

                if (validPosition(a, b) && cells[a, b] == currentTurn)
                {
                    foundFriendlyCombo = true;
                }

                if (foundFriendlyCombo)
                {
                    a = x + xvel;
                    b = y + yvel;

                    while (validPosition(a, b) && cells[a, b] == opposition)
                    {
                        cells[a, b] = currentTurn;

                        a += xvel;
                        b += yvel;
                    }
                }
            }

            updateScores();

            switchTurns();

            if (noPlayPossible())
                switchTurns();

            if (noPlayPossible())
                gameover = true;
        }

        private void updateScores()
        {
            whiteScore = 0;
            blackScore = 0;
            unoccupiedCells = 0;

            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    if (cells[x, y] == WHITE)
                        whiteScore += 1;
                    else if (cells[x, y] == BLACK)
                        blackScore += 1;
                    else
                        unoccupiedCells += 1;
                }
            }
        }

        private bool noPlayPossible()
        {
            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    if (canPlayAtPosition(x, y))
                        return false;
                }
            }

            return true;
        }


    }
}
