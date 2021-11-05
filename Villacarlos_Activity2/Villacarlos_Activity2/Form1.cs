using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Villacarlos_Activity2
{
    public partial class Form1 : Form
    {

        int side;
        int n = 6;
        SixState startState;
        SixState currentState;
        int moveCounter;


        int[,] hTable;
        ArrayList bMoves;
        Object chosenMove;

        public Form1()
        {
            InitializeComponent();
            side = pictureBox1.Width / n;
            startState = randomSixState();
            currentState = new SixState(startState);

            updateUI();
            pictureBox1.Refresh();
            label1.Text = "Attacking pairs: " + getAttackingPairs(startState);

        }

        private int getAttackingPairs(SixState f)
        {
            int attackers = 0;
            for (int rf = 0; rf < n; rf++)
            {
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // get horizontal attackers
                    if (f.Y[rf] == f.Y[tar])
                        attackers++;
                }
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // get diagonal down attackers
                    if (f.Y[tar] == f.Y[rf] + tar - rf)
                        attackers++;
                }
                for (int tar = rf + 1; tar < n; tar++)
                {
                    // get diagonal up attackers
                    if (f.Y[rf] == f.Y[tar] + tar - rf)
                        attackers++;
                }
            }
            return attackers;
        }

        private void updateUI()
        {
           //pictureBox1.Refresh();
           pictureBox2.Refresh();

            label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
            label2.Text = "Attacking pairs: " + getAttackingPairs(currentState);
            label5.Text = "Moves: " + moveCounter;

   
            hTable = getHeuristicTableForPossibleMoves(currentState);
            bMoves = getBestMoves(hTable);

            
            listBox1.Items.Clear();
            foreach (Point move in bMoves)
            {
                listBox1.Items.Add(move);
            }


            //Get next moves
            if (bMoves.Count > 0)
                chosenMove = chooseMove(bMoves);
            label3.Text = "Chosen move: " + chosenMove;
            

        }

        private ArrayList getBestMoves(int[,] heuristicTable)
        {
            ArrayList bestMoves = new ArrayList();
            int bestHeuristicValue = heuristicTable[0, 0];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (bestHeuristicValue > heuristicTable[i, j]) //if the valuis greater than in the 2d array 
                    {
                        bestHeuristicValue = heuristicTable[i, j];  //replace them, this is to ensure that no other queen would be at the same position to the others
                        bestMoves.Clear();                              // ^ this is to make sure that it would prioritize the queens on the leftmost before the rightones
                        if (currentState.Y[i] != j)         //if the old state is not equal to the new one, then add them 
                            bestMoves.Add(new Point(i, j)); 
                    }
                    else if (bestHeuristicValue == heuristicTable[i, j]) //if it is equal 
                    {
                        if (currentState.Y[i] != j)     //and add the same  time it is not equal then add them. 
                            bestMoves.Add(new Point(i, j));
                    }
                }
            }
            label4.Text = "Possible Moves (H=" + bestHeuristicValue + ")";
            return bestMoves;

        }

        
        private int[,] getHeuristicTableForPossibleMoves(SixState newState)
        {
            int[,] hStates = new int[n, n];

            for (int i = 0; i < n; i++) // go through the indices
            {
                for (int j = 0; j < n; j++) // replace them with a new value
                {
                    //Insert code here 
                    SixState temp= new SixState(newState); //create a temporary state for the current state
                    temp.Y[i] = j; //place the old queen to a new position or simply replace them with a new value
                    hStates[i, j] = getAttackingPairs(temp); //count the attacking pairs of that newly position queen
                }
            }

            return hStates;
        }
       

        private SixState randomSixState()
        {
            Random r = new Random();
            SixState random = new SixState(r.Next(n),r.Next(n), r.Next(n), r.Next(n), r.Next(n), r.Next(n));

            return random; 
        }


        private Object chooseMove(ArrayList possibleMoves)
        {
            int arrayLength = possibleMoves.Count;
            Random r = new Random();
            int randomMove = r.Next(arrayLength); // code here next move to choose form the possible moves
            //would just randomly choose a move that is from the array of possible moves that can be done

        return possibleMoves[randomMove];
        }

        private void executeMove(Point move)
        {
            for (int i = 0; i < n; i++)
            {
                startState.Y[i] = currentState.Y[i];
            }
            currentState.Y[move.X] = move.Y;
            moveCounter++;

            chosenMove = null;
            updateUI();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //draw squares 
            for(int i = 0; i<n; i++)
            {
                for(int j=0; j<n; j++)
                {
                    if((i+j)%2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Gray, i * side, j * side, side, side); 
                    }
                    //draw queens
                    if (j == startState.Y[i]){
                        e.Graphics.FillEllipse(Brushes.Red, i * side, j * side, side, side);
                    }
                }
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Gray, i * side, j * side, side, side);
                    }
                    // draw queens
                    if (j == currentState.Y[i])
                        e.Graphics.FillEllipse(Brushes.Red, i * side, j * side, side, side);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            startState = randomSixState();
            currentState = new SixState(startState);

            moveCounter = 0;

            updateUI();
            pictureBox1.Refresh();
            label1.Text = "Attacking pairs: " + getAttackingPairs(startState);
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (getAttackingPairs(currentState) > 0)
                executeMove((Point)chosenMove);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (getAttackingPairs(currentState) > 0)
            {
                executeMove((Point)chosenMove);
            }
        }
    }
}
