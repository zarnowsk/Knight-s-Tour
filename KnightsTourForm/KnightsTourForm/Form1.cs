using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KnightsTourForm
{
    public partial class Form1 : Form
    {
        private static Random random = new Random();
        private static int moveCounter = 1, runCounter = 1, displayCounter;
        private static int startX, startY;
        private static Results results;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Generate a Results object to store game results
            results = new Results();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            // Get starting position from the user
            startX = Convert.ToInt32(rowCoordinate.Value) - 1;
            startY = Convert.ToInt32(colCoordinate.Value) - 1;

            // Get approach and attempts amount from the user
            int choice;
            if (nonIntelRadioBtn.Checked == true)
            {
                choice = 1;
            }
            else
            {
                choice = 2;
            }
            int attempts = Convert.ToInt32(attemptsEntry.Value);

            // Run program for the amount of times entered
            for (int i = 0; i < attempts; i++)
            {
                // Run approach based on choice
                switch (choice)
                {
                    case 1:
                        runNonIntelligent();
                        break;
                    case 2:
                        runIntelligent();
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                // Write run results to file
                writeResultsToFile(choice);

                // Adjust conuters for next run
                moveCounter = 1;
                runCounter++;
            }

            // Remove begin button
            Controls.Remove(startBtn);

            // Display results
            displayCounter = 1;
            runNoLbl.Text = "1";
            touchedLbl.Text = results.CompletedMovesResults[0];
            foreach (Control c in Controls)
            {
                if (c is TextBox)
                {
                    int location = Convert.ToInt32(c.Name.Substring(c.Name.Length - 2));
                    c.Text = results.GameResults[0][location].ToString();
                }
            }

            // Enable buttons to cycle through tours
            nextTourBtn.Visible = true;
            prevTourBtn.Visible = true;

            // Display averge and deviation
            int avg = getAverageRuns();
            int completedMoves = results.CompletedMovesResults[0] == "all" ?
                64 : Convert.ToInt32(results.CompletedMovesResults[0]);
            avgLbl.Text = "Average achieved steps: " + avg.ToString();
            deviationLbl.Text = "Deviation from average: " + (completedMoves - avg).ToString();
        }

        static void runNonIntelligent()
        {
            Console.WriteLine("Run started: " + runCounter);
            ChessBoard board = new ChessBoard();
            Knight knight = new Knight(startX, startY);

            // Handle starting position
            handleStartingPosition(board, knight);

            while (true)
            {
                // Determine next move
                int[] nextMove = determineNextMove(board, knight, 1);

                // If returned move is out of bounds, finish the run
                if (nextMove[0] == 9)
                {
                    break;
                }

                // Make the move
                makeMove(board, knight, nextMove);
            }
            board.showTraversedBoard();

            // Save game results
            saveGameResult(board);

        }

        static void runIntelligent()
        {
            Console.WriteLine("Run started: " + runCounter);
            ChessBoard board = new ChessBoard();
            Knight knight = new Knight(startX, startY);

            // Handle starting position
            handleStartingPosition(board, knight);

            // Adjust accessibilty matrix based on starting position
            adjustAccessibilityMatrix(board, knight);

            while (true)
            {
                // Determine next move
                int[] nextMove = determineNextMove(board, knight, 2);

                // If returned move is out of bounds, finish the run
                if (nextMove[0] == 9)
                {
                    break;
                }

                // Make the move
                makeMove(board, knight, nextMove);

                // Adjust accessibilty matrix based on new position
                adjustAccessibilityMatrix(board, knight);
            }
            board.showTraversedBoard();

            // Save game results
            saveGameResult(board);

        }

        static void handleStartingPosition(ChessBoard board, Knight knight)
        {
            // Mark starting position on the board
            int[] startPosition = { knight.X, knight.Y };
            int indexOfStartPosition = board.Positions.FindIndex(startPosition.SequenceEqual);
            board.TraversedPositions[indexOfStartPosition] = moveCounter++;

            // Mark starting position on the board as used
            board.Positions[indexOfStartPosition] = new int[] { 99, 99 };
        }

        static int[] determineNextMove(ChessBoard board, Knight knight, int runMethod)
        {
            // Create a list to store all possible next moves
            List<int[]> possibleMoves = new List<int[]>();

            // Loop through Knight's all possible moves
            for (int i = 0; i < knight.Moves.Length / 2; i++)
            {
                // Determine next possible move based on current position
                int nextPossibleX = knight.X + knight.Moves[i, 0];
                int nextPossibleY = knight.Y + knight.Moves[i, 1];
                int[] nextPossibleMove = { nextPossibleX, nextPossibleY };

                // Add next possible move to List of possible moves if it's available on the board
                if (board.Positions.Any(a => a.SequenceEqual(nextPossibleMove)))
                {
                    possibleMoves.Add(nextPossibleMove);
                }
            }

            // Return next move based on game approach (non-intelligent/intelligent)
            switch (runMethod)
            {
                case 1:
                    // Generate random number based on number of possible moves
                    int randomIndex = random.Next(0, possibleMoves.Count);

                    // If any moves are possible, return a random one from the list of possible moces
                    if (possibleMoves.Count > 0)
                    {
                        return possibleMoves[randomIndex];
                    }
                    break;
                case 2:
                    // If any moves possible
                    if (possibleMoves.Count > 0)
                    {
                        // GET ACCESSIBILTY VALUES OF ALL BOARD POSITIONS REACHABLE BY THE KNIGHT
                        List<int> accessValsBasedOnIndexes = new List<int>();
                        int smallestIndex;

                        // Loop through possible moves
                        for (int i = 0; i < possibleMoves.Count; i++)
                        {
                            // Get relative board index of the possible move
                            int indexOfPossibleMove = board.Positions.FindIndex(possibleMoves[i].SequenceEqual);
                            // Add accessibilty matrix value of that board position into the list
                            accessValsBasedOnIndexes.Add(board.PositionsAccessibilty[indexOfPossibleMove]);
                        }

                        // DETERMINE ACCESSIBLE BOARD POSITION BASED ON SMALLEST ACCESSIBILTY VALUE
                        smallestIndex = accessValsBasedOnIndexes[0];

                        // Loop through all acquired accessibilty values to find the smallest value
                        for (int i = 0; i < accessValsBasedOnIndexes.Count; i++)
                        {
                            // If accessibilty value is smaller than currently assigned one, 
                            // change it to current accessibilty value
                            if (accessValsBasedOnIndexes[i] < smallestIndex)
                            {
                                smallestIndex = accessValsBasedOnIndexes[i];
                            }
                        }

                        // Loop through all acquired accessibilty values to find all values equal to the 
                        // smallest accessibilty value and add their List indexes to the list
                        List<int> indexesOfSmallestValues = new List<int>();
                        for (int i = 0; i < accessValsBasedOnIndexes.Count; i++)
                        {
                            if (accessValsBasedOnIndexes[i] == smallestIndex)
                            {
                                indexesOfSmallestValues.Add(i);
                            }
                        }

                        // Get randomly selected index of one of the smallest accessibility values
                        int randomVal = random.Next(0, indexesOfSmallestValues.Count);
                        int selectedIndex = indexesOfSmallestValues[randomVal];

                        // Return the move with the randomly selected smallest accessibilty value
                        return possibleMoves[selectedIndex];
                    }
                    break;
                default:
                    break;
            }

            // If no moves were possible, return and out of bounds move
            return new int[] { 9, 9 };
        }

        static void makeMove(ChessBoard board, Knight knight, int[] move)
        {
            // Place knight on new position
            knight.X = move[0];
            knight.Y = move[1];

            // Mark position with move counter
            int indexOfMovePosition = board.Positions.FindIndex(move.SequenceEqual);
            board.TraversedPositions[indexOfMovePosition] = moveCounter++;

            // Remove position from board positions
            board.Positions[indexOfMovePosition] = new int[] { 99, 99 };
        }

        static void writeResultsToFile(int option)
        {
            // Determine file name based on run approach
            string fileName = "";
            if (option == 1)
            {
                fileName += "michalzarnowskiNonIntelligentMethod.txt";
            }
            else if (option == 2)
            {
                fileName += "michalzarnowskiHeuristicMethod.txt";
            }

            // Compose path to file
            var path = System.AppContext.BaseDirectory;
            var filePath = path + fileName;

            // Determine amount of completed moves by the Knight
            String completedMoves = (moveCounter - 1).ToString();
            if (moveCounter - 1 == 64)
            {
                completedMoves = "all";
            }

            // Compose the line to save in the text file
            string printLine = "Trial " + runCounter + ": The Knight was able to successfully touch " + completedMoves + " squares.";

            // Save line to file
            TextWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(printLine);
            writer.Close();

            // Save game results
            results.addCompletedMovesResult(completedMoves);
        }

        static void adjustAccessibilityMatrix(ChessBoard board, Knight knight)
        {
            int[] currentPosition = { knight.X, knight.Y };

            // Loop through Knight's all possible moves
            for (int i = 0; i < knight.Moves.Length / 2; i++)
            {
                // Get next possible move in array format
                int nextPossibleX = knight.X + knight.Moves[i, 0];
                int nextPossibleY = knight.Y + knight.Moves[i, 1];
                int[] nextPossibleMove = { nextPossibleX, nextPossibleY };

                // Find the index of the next move in board's position matrix
                int indexOfPossibleMovePosition = board.Positions.FindIndex(nextPossibleMove.SequenceEqual);

                // If next move possible (not -1), adjust its accessibilty value by subtracting 1
                if (indexOfPossibleMovePosition >= 0)
                {
                    board.PositionsAccessibilty[indexOfPossibleMovePosition] = board.PositionsAccessibilty[indexOfPossibleMovePosition] - 1;
                }

            }
        }

        static void saveGameResult(ChessBoard board)
        {
            int[] resultsArr = board.TraversedPositions.ToArray();
            results.addGameResult(resultsArr);

        }

        private void runNoLbl_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void prevTourBtn_Click(object sender, EventArgs e)
        {
            if (displayCounter > 1)
            {
                // Display results based on adjustd display counter
                displayCounter--;
                displayResults();
            }
        }

        private void nextTourBtn_Click(object sender, EventArgs e)
        {
            if (results.AmountOfStoredResults > displayCounter)
            {
                // Display results based on adjustd display counter
                displayCounter++;
                displayResults();
            }
        }

        void displayResults()
        {
            runNoLbl.Text = displayCounter.ToString();
            touchedLbl.Text = results.CompletedMovesResults[displayCounter - 1];
            foreach (Control c in Controls)
            {
                if (c is TextBox)
                {
                    int location = Convert.ToInt32(c.Name.Substring(c.Name.Length - 2));
                    c.Text = results.GameResults[displayCounter - 1][location].ToString();
                }
            }

            // Display deviation from average
            int completedMoves = results.CompletedMovesResults[displayCounter - 1] == "all" ?
            64 : Convert.ToInt32(results.CompletedMovesResults[displayCounter - 1]);
            deviationLbl.Text = "Deviation from average: " + (completedMoves - getAverageRuns()).ToString();
        }

        static int getAverageRuns()
        {
            int sumOfMoves = 0;
            for(int i = 0; i < results.CompletedMovesResults.Count; i++)
            {
                if (results.CompletedMovesResults[i] == "all")
                {
                    sumOfMoves += 64;
                }
                else
                {
                    sumOfMoves += Convert.ToInt32(results.CompletedMovesResults[i]);
                }
            }

            return sumOfMoves / results.CompletedMovesResults.Count;
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
