using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightsTourForm
{
    class Results
    {
        private int amountOfStoredResutls;
        private List<int[]> gameResults;
        private List<string> completedMovesResults;
        public int AmountOfStoredResults { get { return amountOfStoredResutls; } }
        public List<int[]> GameResults { get { return gameResults; } }
        public List<string> CompletedMovesResults { get { return completedMovesResults; } }

        public Results()
        {
            amountOfStoredResutls = 0;
            gameResults = new List<int[]>();
            completedMovesResults = new List<string>();
        }

        public void addGameResult(int[] gameResult)
        {
            // Increase storage counter
            amountOfStoredResutls++;

            // Add supplied game result to stored results
            gameResults.Add(gameResult);
        }

        public void addCompletedMovesResult(string result)
        {
            completedMovesResults.Add(result);
        }
    }
}
