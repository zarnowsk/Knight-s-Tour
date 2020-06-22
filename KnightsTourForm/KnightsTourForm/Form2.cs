using System;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            backgroundInfoTxt.Text = "A knight's tour is a sequence of moves of a knight on a chessboard" +
                " such that the knight visits every square exactly once. If the knight ends on a square that" +
                " is one knight's move from the beginning square (so that it could tour the board again" +
                " immediately, following the same path), the tour is closed; otherwise, it is open.";

            playInfoTxt1.Text = "1) Select game approach. Non-intelligent will choose Knight's move at random," +
                " intelligent will choose next move based on accessibilty heuristic (higher chance of success).";
            playInfoTxt2.Text = "2) Select how many times you'd like the Knight to attempt the tour.";
            playInfoTxt3.Text = "3) Select the starting board position for the Knight by selecting row and column.";
            playInfoTxt4.Text = "4) Hit Begin tour!";
            playInfoTxt5.Text = "5) Results of the run will be displayed below showing the order of each move. " +
                "Zeros indicate unreached positions.";
        }
    }
}
