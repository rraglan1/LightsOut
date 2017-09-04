using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lights_Out
{
	public partial class MainForm : Form
	{
		private const int GridOffset = 25;      //distance from upper-left side of window
		private const int GridLength = 200;     //size in pixels of grid
		private const int NumCells = 3;         //number of cells in grid
		private const int CellLength = GridLength / NumCells;

		private bool[,] grid;					//stores on/off state of cells in grid
		private Random rand;					//used to generate random numbers

		public MainForm()
		{
			InitializeComponent();

			rand = new Random();        //Initialize random number generation

			grid = new bool[NumCells, NumCells];

			//Turn entire grid on
			for (int r = 0; r < NumCells; r++)
			{
				for (int c = 0; c < NumCells; c++)
				{
					grid[r, c] = true;
				}
			}
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			newGameButton_Click(sender, e);
		}

		private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			for (int r = 0; r < NumCells; r++)
			{
				for (int c = 0; c < NumCells; c++)
				{
					//Get proper pen/brush for on/off
					//grid selection
					Brush brush;
					Pen pen;

					if (grid[r,c ])
					{
						pen = Pens.Black;
						brush = Brushes.White;		//on
					}
					else
					{
						pen = Pens.White;
						brush = Brushes.Black;		//off
					}

					//Determine (x, y) coord of row and col to draw rectangle
					int x = c * CellLength + GridOffset;
					int y = r * CellLength + GridOffset;

					//Draw outline and inner rectangle
					g.DrawRectangle(pen, x, y, CellLength, CellLength);
					g.FillRectangle(brush, x + 1, y + 1, CellLength - 1, CellLength - 1);
				}
			}
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e)
		{
			//Make sure click was inside the grid
			if (e.X < GridOffset || e.X > CellLength * NumCells + GridOffset ||
				e.Y < GridOffset || e.Y > CellLength * NumCells + GridOffset)
			{
				return;
			}

			//Find row, col of mouse press
			int r = (e.Y - GridOffset) / CellLength;
			int c = (e.X - GridOffset) / CellLength;

			//Invert selescted box and all surrounding boxes
			for (int i = r-1; i <= r+1; i++)
			{
				for (int j = c-1; j <= c+1; j++)
				{
					if (i >= 0 && i < NumCells && j >= 0 && j < NumCells)
					{
						grid[i, j] = !grid[i, j];
					}
				}
			}

			//Redraw grid
			this.Invalidate();

			//Check to see if puzzle has been solved
			if (PlayerWon())
			{
				//Display Winner dialog box
				MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
									MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		private bool PlayerWon()
		{
			int counter = 0;
			for (int r = 0; r < NumCells; r++)
			{
				for (int c = 0; c < NumCells; c++)
				{
					if (grid[r, c] == false)
						counter++;
				}
			}

			if (counter == NumCells * NumCells)
				return true;
			else
				return false;
		}

		private void newGameButton_Click(object sender, EventArgs e)
		{
			//Fill grid with either white or black
			for (int r = 0; r < NumCells; r++)
			{
				for (int c = 0; c < NumCells; c++)
				{
					grid[r, c] = rand.Next(2) == 1;
				}
			}

			//Redraw Grid
			this.Invalidate();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void exitButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm aboutBox = new AboutForm();
			aboutBox.ShowDialog(this);
		}
	}

}

