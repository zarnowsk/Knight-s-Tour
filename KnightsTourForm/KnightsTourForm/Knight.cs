using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTourForm
{
    class Knight : ChessPiece
    {
        private int x;
        private int y;
        private int[,] moves = { { 2, -1 }, { 1, -2 }, { -1, -2 }, { -2, -1 }, { -2, 1 }, { -1, 2 }, { 1, 2 }, { 2, 1 } };

        public int X { get { return x; } set { x = rangeCheck(value); } }
        public int Y { get { return y; } set { y = rangeCheck(value); } }
        public int[,] Moves { get { return moves; } }

        public Knight(int x, int y)
        {
            this.x = rangeCheck(x);
            this.y = rangeCheck(y);
        }
    }
}
