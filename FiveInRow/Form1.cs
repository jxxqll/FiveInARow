using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiveInRow
{
    public partial class Form1 : Form
    {
        #region Consts
        /// <summary>
        /// Max radius for a stone
        /// </summary>
        const int MAX_RADIUS = 30;
        /// <summary>
        /// Drawing radius for a stone
        /// </summary>
        const int STONE_DRAWING_RADIUS = 25;
        /// <summary>
        /// Sensing radius for a stone
        /// </summary>
        const int SENSING_RADIUS = 20;
        /// <summary>
        ///  Length for the Board Square 
        /// </summary>
        const int BOARD_SIZE = 900;
        /// <summary>
        /// Radius for dot in the center
        /// </summary>
        const int DOT_RADIUS = 10;

        #endregion
        /// <summary>
        /// Board object
        /// </summary>
        Board _board;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _board = new Board();
        }

        /// <summary>
        /// Axis Calculation
        /// </summary>
        /// <param name="XorY"></param>
        /// <returns></returns>
        static int CalculateAxis(int XorY)
        {
            int i = XorY / (2 * MAX_RADIUS);

            if (i * 2 * MAX_RADIUS + MAX_RADIUS - SENSING_RADIUS <= XorY && XorY <= i * 2 * MAX_RADIUS + MAX_RADIUS + SENSING_RADIUS)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Draw Game Board
        /// </summary>
        void DrawBoard()
        {
            Graphics g = this.CreateGraphics();

            /*
            #region Draw Background
            //Draw Background Color
            SolidBrush myBrush = new SolidBrush(Color.Red);
            g.FillRectangle(myBrush, new Rectangle(0, 0, BOARD_SIZE, BOARD_SIZE));
            #endregion
            */

            #region Draw grids
            
            Pen penBorder = new Pen(Color.Black, 2.5f);
            //g.DrawRectangle(pen, new Rectangle(new Point(radius, radius), new Size(900 - 2 * radius, 900 - 2 * radius)));

            for (int i = 0; MAX_RADIUS + i * 2 * MAX_RADIUS < BOARD_SIZE; i++)
            {
                //draw horizontal lines
                g.DrawLine(penBorder, new Point(MAX_RADIUS + i * 2 * MAX_RADIUS, MAX_RADIUS), new Point(MAX_RADIUS + i * 2 * MAX_RADIUS, BOARD_SIZE - MAX_RADIUS));
                //draw vertical lines
                g.DrawLine(penBorder, new Point(MAX_RADIUS, MAX_RADIUS + i * 2 * MAX_RADIUS), new Point(BOARD_SIZE - MAX_RADIUS, MAX_RADIUS + i * 2 * MAX_RADIUS));
            }
            #endregion


            #region Draw 5 dots
            Pen penDot = new Pen(Color.Black, 10);

            g.DrawEllipse(penDot, BOARD_SIZE / 2 - DOT_RADIUS / 2, BOARD_SIZE / 2 - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 3 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, 3 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 11 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, 11 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 3 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, 11 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 11 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, 3 * 2 * MAX_RADIUS + MAX_RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            #endregion

        }

        /// <summary>
        /// Draw Stone
        /// </summary>
        /// <param name="x">X-Axis</param>
        /// <param name="y">Y-Axis</param>
        void DrawStone(int x, int y)
        {
            DrawStone(x, y, _board.CurrentMoveColor == StoneColor.BALCK ? Color.Black : Color.White);
        }
        /// <summary>
        /// Draw Stone
        /// </summary>
        /// <param name="x">X-Axis</param>
        /// <param name="y">Y-Axis</param>
        /// <param name="c">Color</param>
        void DrawStone(int x, int y, Color c)
        {
            Point center = new Point(MAX_RADIUS + x * 2 * MAX_RADIUS, MAX_RADIUS + y * 2 * MAX_RADIUS);

            Graphics g = this.CreateGraphics();
            g.FillEllipse(new SolidBrush(c), center.X - STONE_DRAWING_RADIUS, center.Y - STONE_DRAWING_RADIUS, 2 * STONE_DRAWING_RADIUS, 2 * STONE_DRAWING_RADIUS);
        }

        #region Events

        /// <summary>
        /// MouseUp Event for Dropping a stone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //Left click to drop a Stone
            if (e.Button == MouseButtons.Left)
            {
                //get click point
                Point p = e.Location;
                this.Text = string.Format("X: {0}, Y:{1}", p.X, p.Y);

                int x = CalculateAxis(p.X); //X-Axis
                int y = CalculateAxis(p.Y); //Y-Axis

                if (!_board.IsGameOver && x > 0 && y > 0) //game is not over and (x,y) is a valid value
                {
                    if (_board.Drop(x, y)) //is able to drop to that point
                    {
                        //Draw stone
                        DrawStone(x, y);

                        try
                        {
                            // Judge game
                            if (this._board.Judge())
                            {
                                this._board.IsGameOver = true;
                                MessageBox.Show(string.Format("{0} wins", this._board.CurrentMoveColor == StoneColor.BALCK ? "Black" : "White"));
                            }
                        }
                        catch (FoulException ex)
                        {
                            MessageBox.Show("this is a fault");
                        }
                    }
                }
            }

            //Right click to regret last move
            if (e.Button == MouseButtons.Right && _board.Moves.Count>0)
            {
                //get click point
                Point p = e.Location;
                this.Text = string.Format("X: {0}, Y:{1}", p.X, p.Y);

                int x = CalculateAxis(p.X); //X-Axis
                int y = CalculateAxis(p.Y); //Y-Axis

                int[] lastMove = _board.Moves.Last();
                //Tell if current right click is on the last move
                if (x == lastMove[0] && y == lastMove[1] && (int)_board.CurrentMoveColor == lastMove[2])
                {
                    _board.RemoveLastMove();
                    //redraw
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Redraw Board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Redraw();
        }

        /// <summary>
        /// Draw / Redraw the board
        /// </summary>
        void Redraw()
        {
            DrawBoard();

            int[] data;
            for (int i = 0; i < this._board.StoneCount; i++)
            {
                data = this._board.Moves[i];
                DrawStone(data[0], data[1], data[2] == 1 ? Color.Black : Color.White);
            }
        }

        /// <summary>
        /// ToolStripMenu click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        /// <summary>
        /// ToolStripMenu click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regret();
        }

        private void progameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _board.IsProGame = true;
        }

        /// <summary>
        /// Hot Key Cntrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    StartNewGame();
                    break;
                case Keys.F3:
                    Regret();
                    break;
                case Keys.F4:
                    Exit();
                    break;
            }
        }
        #endregion


        #region GameControlFunctions
        /// <summary>
        /// Regret
        /// </summary>
        void Regret()
        {
            if (!this._board.IsGameOver && this._board.StoneCount > 0)
            {
                this.Invalidate();
                this._board.RemoveLastMove();
            }
        }

        /// <summary>
        /// Start new game
        /// </summary>
        void StartNewGame()
        {
            MBox bx = new MBox("Start new Game?");
            bx.StartPosition = FormStartPosition.CenterParent;
            var result = bx.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this._board = new Board();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Exit Game
        /// </summary>
        void Exit()
        {
            MBox bx = new MBox("Exit?");
            bx.StartPosition = FormStartPosition.CenterParent;
            var result = bx.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        #endregion

        
    }
}
