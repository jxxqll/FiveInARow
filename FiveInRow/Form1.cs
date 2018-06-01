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
        const int RADIUS = 30;
        const int SENCE_RADIUS = 20;
        const int STONE_RADIUS = 25;
        const int BOARD_SIZE = 900;
        const int DOT_RADIUS = 10;

        Board _board;

        public Form1()
        {
            InitializeComponent();
            _board = new Board();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point p = e.Location;
                this.Text = string.Format("X: {0}, Y:{1}", p.X, p.Y);

                int x = CalDot(p.X);
                int y = CalDot(p.Y);

                if (x == -1 || y == -1)//misclick
                {

                }
                else if (!this._board.IsGameOver)
                {
                    if (_board.Drop(x, y))
                    {
                        DrawStone(x, y, _board.CurrentMoveColor ? Color.Black : Color.White);
                        try
                        {
                            if (this._board.Judge())
                            {
                                this._board.IsGameOver = true;
                                MessageBox.Show(string.Format("{0} wins", this._board.CurrentMoveColor ? "Black" : "White"));
                            }
                        }
                        catch (FaultException ex)
                        {
                            MessageBox.Show("this is a fault");
                        }
                    }
                }
            }
        }

        static int CalDot(int XorY)
        {
            int i = XorY / (2 * RADIUS);

            if (i * 2 * RADIUS + RADIUS - SENCE_RADIUS <= XorY && XorY <= i * 2 * RADIUS + RADIUS + SENCE_RADIUS)
            {
                return i;
            }
            else
            {
                return -1;
            }
        }

        void DrawBoard()
        {
            Graphics g = this.CreateGraphics();
            Pen penBorder = new Pen(Color.Black, 2.5f);
            //g.DrawRectangle(pen, new Rectangle(new Point(radius, radius), new Size(900 - 2 * radius, 900 - 2 * radius)));

            for (int i = 0; RADIUS + i * 2 * RADIUS < BOARD_SIZE; i++)
            {
                //draw horizontal lines
                g.DrawLine(penBorder, new Point(RADIUS + i * 2 * RADIUS, RADIUS), new Point(RADIUS + i * 2 * RADIUS, BOARD_SIZE - RADIUS));
                //draw vertical lines
                g.DrawLine(penBorder, new Point(RADIUS, RADIUS + i * 2 * RADIUS), new Point(BOARD_SIZE - RADIUS, RADIUS + i * 2 * RADIUS));
            }

            Pen penDot = new Pen(Color.Black, 10);

            g.DrawEllipse(penDot, BOARD_SIZE / 2 - DOT_RADIUS / 2, BOARD_SIZE / 2 - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 3 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, 3 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 11 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, 11 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 3 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, 11 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
            g.DrawEllipse(penDot, 11 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, 3 * 2 * RADIUS + RADIUS - DOT_RADIUS / 2, DOT_RADIUS, DOT_RADIUS);
        }

        void DrawStone(int x, int y, Color c)
        {
            Point center = new Point(RADIUS + x * 2 * RADIUS, RADIUS + y * 2 * RADIUS);

            Graphics g = this.CreateGraphics();
            g.FillEllipse(new SolidBrush(c), center.X - STONE_RADIUS, center.Y - STONE_RADIUS, 2 * STONE_RADIUS, 2 * STONE_RADIUS);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawBoard();

            int[] data;
            for (int i = 0; i < this._board.StoneCount; i++)
            {
                data = this._board.Moves[i];
                DrawStone(data[0], data[1], data[2] == 1 ? Color.Black : Color.White);
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void regretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regret();
        }

        void Regret()
        {
            if (!this._board.IsGameOver && this._board.StoneCount > 0)
            {
                this.Invalidate();
                this._board.RemoveLastMove();
            }
        }

        void NewGame()
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

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    NewGame();
                    break;
                case Keys.F3:
                    Regret();
                    break;
                case Keys.F4:
                    Exit();
                    break;
            }
        }
    }
}
