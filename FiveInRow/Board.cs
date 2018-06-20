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


        // Constructor
        public Board()
        {
            // initialise
            StoneCount = 0;
            IsProGame = false;

            // set every point to empty
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
            if (Moves.Count < 9)
            {
                return false;
            }

            //judge 5
            int[] last = this.Moves.Last();
            int x = last[0], y = last[1];

            int[] result = new int[4];

            result[0] = CountHorizontal(x, y, 0);
            result[1] = CountVertical(x, y, 0);
            result[2] = CountSlash(x, y, 0);
            result[3] = CountBackSlash(x, y, 0);

            //pro game & black
            if (this.IsProGame && this.CurrentMoveColor == StoneColor.BALCK)
            {
                //if 5-connected Black Win anyway
                if (result.Contains(5))
                {
                    return true;
                }

                //judge over5
                if (result.Max() > 5)
                {
                    throw new FoulException(FoulTypes.over5);
                }
                else
                {
                    Judge44Foul(x, y);
                }
            }
            else //simple game
            {
                return result.Max() >= 5;
            }

            return false;
        }

        int CountHorizontal(int x, int y, int countBreakLimmit)
        {
            return Count(x, y, countBreakLimmit, (int c) => { return --c; }, null, (int c) => { return ++c; }, null);
        }

        int CountVertical(int x, int y, int countBreakLimmit)
        {
            return Count(x, y, countBreakLimmit, null, (int c) => { return --c; }, null, (int c) => { return ++c; });
        }

        int CountSlash(int x, int y, int countBreakLimmit)
        {
            return Count(x, y, countBreakLimmit, (int c) => { return --c; }, (int c) => { return ++c; }, (int c) => { return ++c; }, (int c) => { return --c; });
        }

        int CountBackSlash(int x, int y, int countBreakLimmit)
        {
            return Count(x, y, countBreakLimmit, (int c) => { return --c; }, (int c) => { return --c; }, (int c) => { return ++c; }, (int c) => { return ++c; });
        }

        void Count(int x, int y, ref int count, ref int countBreak, int countBreakLimmit, Func<int, int> countUpX, Func<int, int> countUpY)
        {
            while (true)
            {
                if (countUpX != null)
                {
                    x = countUpX(x);
                }
                if (countUpY != null)
                {
                    y = countUpY(y);
                }

                if (x < 0 || x > 14 || y < 0 || y > 14)
                {
                    break;
                }
                else
                {
                    if (board[x, y] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[x, y] == StoneColor.EMPTY && countBreak < countBreakLimmit)
                    {
                        countBreak++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        int Count(int x, int y, int countBreak, int countBreakLimmit, Func<int, int> countUpX1, Func<int, int> countUpY1, Func<int, int> countUpX2, Func<int, int> countUpY2)
        {
            int count = 1;

            Count(x, y, ref count, ref countBreak, countBreakLimmit, countUpX1, countUpY1);
            Count(x, y, ref count, ref countBreak, countBreakLimmit, countUpX2, countUpY2);

            return count;
        }

        int Count(int x, int y, int countBreakLimmit, Func<int, int> countUpX1, Func<int, int> countUpY1, Func<int, int> countUpX2, Func<int, int> countUpY2)
        {
            int count = 0;

            int countA = Count(x, y, 0, countBreakLimmit, countUpX1, countUpY1, countUpX2, countUpY2);

            if (countBreakLimmit > 0)
            {
                int countB = Count(x, y, 0, countBreakLimmit, countUpX2, countUpY2, countUpX1, countUpY1);
                count = countA > countB ? countA : countB;
            }
            else
            {
                count = countA;
            }

            return count;
        }

        void Judge44Foul(int x, int y)
        {
            int[] result = new int[4];

            result[0] = CountHorizontal(x, y, 1);
            result[1] = CountVertical(x, y, 1);
            result[2] = CountSlash(x, y, 1);
            result[3] = CountBackSlash(x, y, 1);

            if (result.Count(r => r == 4) >= 2)
            {
                throw new FoulException(FoulTypes.double4);
            }
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
