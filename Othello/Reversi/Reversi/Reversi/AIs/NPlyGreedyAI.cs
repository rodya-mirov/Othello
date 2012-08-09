using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reversi.AIs
{
    public class NPlyGreedyAI : NPlyAI
    {
        public NPlyGreedyAI(int n)
            : base(n)
        {
        }

        protected override int evaluateBoard(int currentTurn, GameBoard result)
        {
            return (currentTurn == GameBoard.WHITE) ? result.whiteScore : result.blackScore;
        }
    }
}
