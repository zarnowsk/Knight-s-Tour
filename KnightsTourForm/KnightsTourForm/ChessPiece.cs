using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTourForm
{
    class ChessPiece
    {
        public int rangeCheck(int val)
        {
            if (val < 0 || val > 7)
            {
                throw new ArgumentOutOfRangeException(val.ToString());
            }
            return val;
        }
    }
}
