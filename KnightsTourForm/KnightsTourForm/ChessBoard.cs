using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTourForm
{
    class ChessBoard : GameBoard
    {
        private int width = 8;
        private int height = 8;
        private List<int[]> positions;
        private List<int> traversedPositions;
        private int[] positionsAccessibilty = { 2, 3, 4, 4, 4, 4, 3, 2,
                                                3, 4, 6, 6, 6, 6, 4, 3,
                                                4, 6, 8, 8, 8, 8, 6, 4,
                                                4, 6, 8, 8, 8, 8, 6, 4,
                                                4, 6, 8, 8, 8, 8, 6, 4,
                                                4, 6, 8, 8, 8, 8, 6, 4,
                                                3, 4, 6, 6, 6, 6, 4, 3,
                                                2, 3, 4, 4, 4, 4, 3, 2,};

        public List<int[]> Positions { get { return positions; } }
        public List<int> TraversedPositions { get { return traversedPositions; } }
        public int[] PositionsAccessibilty { get { return positionsAccessibilty; } }

        public ChessBoard()
        {
            // Initialize positions List with board coordinates
            // and traversdedPositions List with empty spots
            positions = new List<int[]>();
            traversedPositions = new List<int>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int[] tempArray = { i, j };
                    positions.Add(tempArray);

                    traversedPositions.Add(0);
                }
            }
        }

        public override void showBoard()
        {
            int counter = 0, listLen = positions.Count;

            while (counter < listLen)
            {
                Console.Write("[{0} {1}] ", positions[counter][0].ToString(), positions[counter][1].ToString());
                if (counter != 0 && (counter + 1) % 8 == 0)
                    Console.WriteLine();
                counter++;
            }
        }

        public override void showTraversedBoard()
        {
            int counter = 0, listLen = traversedPositions.Count;

            while (counter < listLen)
            {
                Console.Write("[{0}] ", traversedPositions[counter].ToString());

                // Spaces formatting
                if (traversedPositions[counter] / 10 < 1)
                {
                    Console.Write(" ");
                }

                // New line formatting
                if (counter != 0 && (counter + 1) % 8 == 0)
                    Console.WriteLine();

                counter++;
            }
        }

        public void showAccessibiltyMatrix()
        {
            int counter = 0, listLen = positionsAccessibilty.Length;

            while (counter < listLen)
            {
                Console.Write("[{0}] ", positionsAccessibilty[counter].ToString());

                // New line formatting
                if (counter != 0 && (counter + 1) % 8 == 0)
                    Console.WriteLine();

                counter++;
            }
        }

    }
}
