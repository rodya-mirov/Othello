using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reversi.AIs
{
    public abstract class AI
    {
        public Point desiredMove;
        public GameBoard storedBoard;

        public abstract void getDesiredMove();

        protected int match(int colorOne, int colorTwo)
        {
            if (colorOne == colorTwo)
                return 1;
            else
                return 0;
        }
    }
}
