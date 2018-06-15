using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRow
{
    /// <summary>
    /// Board Class
    /// </summary>
    public class Board
    {
        // BoardSize
        public const int BoardSize = 15;

        // board
        private readonly int[,] board = new int[BoardSize, BoardSize];


        /// <summary>
        /// Num of stones
        /// </summary>
        public int StoneCount { get; set; }

        /// <summary>
        /// Current move color. 1: black move / 0: white move
        /// </summary>
        public int CurrentMoveColor
        {
            get { return (this.StoneCount % 2); }
        }
        /// <summary>
        /// Game over flag
        /// </summary>
        public bool IsGameOver { get; set; }
        /// <summary>
        /// Is this a pro game
        /// </summary>
        public bool IsProGame { get; set; }
        /// <summary>
        /// Move list
        /// </summary>
        public List<int[]> Moves { get; } = new List<int[]>(225);


        /// <summary>
        /// Constructor
        /// </summary>
        public Board()
        {
            //initialise
            StoneCount = 0;
            IsProGame = false;

            //set every point to empty
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = StoneColor.EMPTY;
                }
            }
        }

        /// <summary>
        /// Drop a Stone onto an empty point
        /// </summary>
        /// <param name="x">X-Axis</param>
        /// <param name="y">Y-Axis</param>
        /// <returns>True: allow to drop that point / False: Unable to drop to that point</returns>
        public bool Drop(int x, int y)
        {
            //only allow drops to an empty point
            if (board[x, y] == StoneColor.EMPTY)
            {
                InsertMove(x, y);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Insert a new move
        /// </summary>
        /// <param name="x">X-Axis</param>
        /// <param name="y">Y-Axis</param>
        private void InsertMove(int x, int y)
        {
            //count up num of stone
            this.StoneCount++;

            //create stone info
            int[] data = new int[3];
            data[0] = x;
            data[1] = y;
            data[2] = this.CurrentMoveColor;

            ///add stone to move
            this.Moves.Add(data);

            //add stone to board
            board[x, y] = this.CurrentMoveColor;
        }

        /// <summary>
        /// Remove last move
        /// </summary>
        public void RemoveLastMove()
        {
            int[] last = this.Moves[this.StoneCount - 1];
            board[last[0], last[1]] = StoneColor.EMPTY;
            this.Moves.RemoveAt(this.StoneCount - 1);
            this.StoneCount--;
        }

        #region Game Judging Methods
        /// <summary>
        /// Judge whether the game is over
        /// </summary>
        /// <returns>true: game over / false: continue to game</returns>
        public bool Judge()
        {
            //judge 5
            int[] last = this.Moves.Last();
            int x = last[0], y = last[1];

            int[] result = new int[4];

            result[0] = JudgeHorizontal(x, y);
            result[1] = JudgeVertical(x, y);
            result[2] = JudgeLeftSlash(x, y);
            result[3] = JudgeRightSlash(x, y);

            if (result.Contains(5))
                return true;

            //pro game & black
            if (this.IsProGame && this.CurrentMoveColor == StoneColor.BALCK)
            {
                //judge over5
                if (result.Max() > 5)
                {
                    throw new FoulException(FoulTypes.over5);
                }
                else
                {

                }
            }
            else //simple game
            {
                return result.Max() >= 5;
            }

            return false;
        }

        #region Judge4Directions
        int JudgeLeftSlash(int x, int y)
        {
            int b = x + y;
            int leftLimit = b - 14 > 0 ? b - 14 : 0;
            int rightLimit = 14 > b ? b : 14;

            leftLimit = leftLimit > x - 4 ? leftLimit : x - 4;
            rightLimit = rightLimit < x + 4 ? rightLimit : x + 4;

            int xl = leftLimit;
            int count = 0, Max = 0;

            while (xl <= rightLimit)
            {
                if (board[xl, b - xl] == this.CurrentMoveColor)
                {
                    count++;
                    if (count > Max)
                    {
                        Max = count;
                    }
                }
                else
                {
                    count = 0;
                }
                xl++;
            }
            return Max;
        }

        int JudgeRightSlash(int x, int y)
        {
            int b = y - x;

            int leftLimit = -b > 0 ? -b : 0;
            int rightLimit = 14 - b > 14 ? 14 : 14 - b;
            leftLimit = leftLimit < x - 4 ? x - 4 : leftLimit;
            rightLimit = rightLimit < x + 4 ? rightLimit : x + 4;

            int xl = leftLimit;
            int count = 0, Max = 0;

            while (xl <= rightLimit)
            {
                if (board[xl, b + xl] == this.CurrentMoveColor)
                {
                    count++;
                    if (count > Max)
                    {
                        Max = count;
                    }
                }
                else
                {
                    count = 0;
                }
                xl++;
            }
            return Max;
        }

        int JudgeHorizontal(int x, int y)
        {
            int count = 1;
            int tempX = x;

            while (true)
            {
                tempX--;
                if (tempX < 0 || board[tempX, y] != CurrentMoveColor)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }

            tempX = x;

            while (true)
            {
                tempX++;
                if (tempX > 14 || board[tempX, y] != CurrentMoveColor)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }

            return count;
        }

        int JudgeVertical(int x, int y)
        {

            int leftLimit = y - 4 > 0 ? y - 4 : 0;
            int rightLimit = y + 4 > 14 ? 14 : y + 4;

            int yl = leftLimit;
            int count = 0, Max = 0;

            while (yl <= rightLimit)
            {
                if (board[x, yl] == this.CurrentMoveColor)
                {
                    count++;
                    if (count > Max)
                    {
                        Max = count;
                    }
                }
                else
                {
                    count = 0;
                }
                yl++;
            }
            return Max;
        }
        #endregion

        bool Judge44(int x, int y)
        {
            return false;
        }

        int JudgeLeftSlash44(int x, int y)
        {
            int b = x + y;
            int leftLimit = b - 14 > 0 ? b - 14 : 0;
            int rightLimit = 14 > b ? b : 14;

            int xl = leftLimit;
            int count = 0, Max = 0;

            while (xl <= rightLimit)
            {
                if (board[xl, b - xl] == this.CurrentMoveColor)
                {
                    count++;
                    if (count > Max)
                    {
                        Max = count;
                    }
                }
                else
                {
                    count = 0;
                }
                xl++;
            }
            return Max;
        }

        int JudgeRightSlash44(int x, int y)
        {
            int b = y - x;

            int leftLimit = -b > 0 ? -b : 0;
            int rightLimit = 14 - b > 14 ? 14 : 14 - b;
            leftLimit = leftLimit < x - 4 ? x - 4 : leftLimit;
            rightLimit = rightLimit < x + 4 ? rightLimit : x + 4;

            int xl = leftLimit;
            int count = 0, Max = 0;

            while (xl <= rightLimit)
            {
                if (board[xl, b + xl] == this.CurrentMoveColor)
                {
                    count++;
                    if (count > Max)
                    {
                        Max = count;
                    }
                }
                else
                {
                    count = 0;
                }
                xl++;
            }
            return Max;
        }

        int JudgeHorizontal44(int x, int y)
        {
            int leftLimit = x - 4 > 0 ? x - 4 : 0;
            int rightLimit = x + 4 > 14 ? 14 : x + 4;

            int xl = leftLimit;
            int count = 0, Max = 0;

            while (xl <= rightLimit)
            {
                if (board[xl, y] == this.CurrentMoveColor)
                {
                    count++;
                    if (count > Max)
                    {
                        Max = count;
                    }
                }
                else
                {
                    count = 0;
                }
                xl++;
            }
            return Max;
        }

        int JudgeVertical44(int x, int y)
        {

            int leftLimit = y - 4 > 0 ? y - 4 : 0;
            int rightLimit = y + 4 > 14 ? 14 : y + 4;

            int yl = leftLimit;
            int count = 0, Max = 0;

            while (yl <= rightLimit)
            {
                if (board[x, yl] == this.CurrentMoveColor)
                {
                    count++;
                    if (count > Max)
                    {
                        Max = count;
                    }
                }
                else
                {
                    count = 0;
                }
                yl++;
            }
            return Max;
        }


        #endregion

    }

    /// <summary>
    /// Is current move a foul
    /// </summary>
    public class FoulException : Exception
    {
        public FoulTypes type { get; set; }

        public FoulException(FoulTypes foulType)
        {
            type = foulType;
        }
    }

    /// <summary>
    /// Type of foul
    /// </summary>
    public enum FoulTypes
    {
        double3,
        double4,
        over5
    }

}
