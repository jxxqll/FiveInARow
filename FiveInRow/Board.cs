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

            result[0] = CountHorizontal(x, y);
            result[1] = CountVertical(x, y);
            result[2] = CountSlash(x, y);
            result[3] = CountBackSlash(x, y);

         
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

       

    

        int CountHorizontal(int x, int y)
        {
            return CountHorizontal(x, y, 0, 0);
        }
        int CountHorizontal(int x, int y, int countBreak, int countBreakLimmit)
        {
            int count = 1;
            int tempX = x;

            //count left part
            while (true)
            {
                tempX--;

                if (tempX < 0)
                {
                    break;
                }
                else
                {
                    if (board[tempX, y] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[tempX, y] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }

            //reset
            tempX = x;

            //count right part
            while (true)
            {
                tempX++;

                if (tempX > 14)
                {
                    break;
                }
                else
                {
                    if (board[tempX, y] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[tempX, y] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }

            return count;
        }

        int CountVertical(int x, int y)
        {
            return CountVertical(x, y, 0, 0);
        }
        int CountVertical(int x, int y, int countBreak, int countBreakLimmit)
        {
            int count = 1;
            int tempY = y;

            while (true)
            {
                tempY--;

                if (tempY < 0)
                {
                    break;
                }
                else
                {
                    if (board[x, tempY] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[x, tempY] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //reset
            tempY = y;
            
            while (true)
            {
                tempY++;

                if (tempY > 14)
                {
                    break;
                }
                else
                {
                    if (board[x, tempY] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[x, tempY] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return count;
        }

        int CountSlash(int x, int y)
        {
            return CountSlash(x, y, 0, 0);
        }
        int CountSlash(int x, int y, int countBreak, int countBreakLimmit)
        {
            int count = 1;
            int tempX = x;
            int tempY = y;

            while (true)
            {
                tempX--;
                tempY++;

                if (tempX < 0 || tempY > 14)
                {
                    break;
                }
                else
                {
                    if (board[tempX, tempY] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[tempX, tempY] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //reset
            tempX = x;
            tempY = y;

            while (true)
            {
                tempX++;
                tempY--;

                if (tempX > 14 || tempY < 0)
                {
                    break;
                }
                else
                {
                    if (board[tempX, tempY] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[tempX, tempY] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return count;
        }

        int CountBackSlash(int x, int y)
        {
            return CountBackSlash(x, y, 0, 0);
        }
        int CountBackSlash(int x, int y, int countBreak, int countBreakLimmit)
        {
            int count = 1;
            int tempX = x;
            int tempY = y;

            while (true)
            {
                tempX--;
                tempY--;

                if (tempX < 0 || tempY < 0)
                {
                    break;
                }
                else
                {
                    if (board[tempX, tempY] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[tempX, tempY] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //reset
            tempX = x;
            tempY = y;
            
            while (true)
            {
                tempX++;
                tempY++;

                if (tempX > 14 || tempY > 14)
                {
                    break;
                }
                else
                {
                    if (board[tempX, tempY] == CurrentMoveColor)
                    {
                        count++;
                    }
                    else if (board[tempX, tempY] == StoneColor.EMPTY)
                    {
                        if (countBreak < countBreakLimmit)
                        {
                            countBreak++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return count;
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
