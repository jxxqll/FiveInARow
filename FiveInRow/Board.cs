using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRow
{
    public class Board
    {
        private Stone[,] _stones = new Stone[15, 15];
        private readonly List<int[]> _moves = new List<int[]>(225);

        public int StoneCount { get; set; }
        public bool CurrentMoveColor
        {
            get
            {
                return this.StoneCount % 2 != 0;
            }
        }
        public bool IsGameOver { get; set; }
        public bool IsProGame { get; set; }

        public List<int[]> Moves
        {
            get
            {
                return this._moves;
            }
        }


        public Board()
        {
            StoneCount = 0;
            IsGameOver = false;
            IsProGame = false;
        }

        public bool Drop(int x, int y)
        {
            if (_stones[x, y] == null)
            {
                InsertMove(x, y);
                return true;
            }
            return false;
        }

        private void InsertMove(int x, int y)
        {
            this.StoneCount++;
            int[] data = new int[3];
            data[0] = x;
            data[1] = y;
            data[2] = this.CurrentMoveColor ? 1 : 0;
            this._moves.Add(data);
            _stones[x, y] = new Stone { Color = this.CurrentMoveColor };

        }

        public void RemoveLastMove()
        {
            int[] last = this._moves[this.StoneCount - 1];
            _stones[last[0], last[1]] = null;
            this._moves.RemoveAt(this.StoneCount - 1);
            this.StoneCount--;
        }

        public bool Judge()
        {
            //judge 5
            int[] last = this._moves.Last();
            int x = last[0], y = last[1];

            int[] result = new int[4];

            result[0] = JudgeHorizontal(x, y);
            result[1] = JudgeVertical(x, y);
            result[2] = JudgeLeftSlash(x, y);
            result[3] = JudgeRightSlash(x, y);

            if (result.Contains(5))
                return true;

            //pro game & black
            if (this.IsProGame && this.CurrentMoveColor)
            {
                //judge over5
                if (result.Max() > 5)
                {
                    throw new FaultException(FaultType.over5);
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
                if (_stones[xl, b - xl] != null && _stones[xl, b - xl].Color == this.CurrentMoveColor)
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
                if (_stones[xl, b + xl] != null && _stones[xl, b + xl].Color == this.CurrentMoveColor)
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
            int leftLimit = x - 4 > 0 ? x - 4 : 0;
            int rightLimit = x + 4 > 14 ? 14 : x + 4;

            int xl = leftLimit;
            int count = 0, Max = 0;

            while (xl <= rightLimit)
            {
                if (_stones[xl, y] != null && _stones[xl, y].Color == this.CurrentMoveColor)
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

        int JudgeVertical(int x, int y)
        {

            int leftLimit = y - 4 > 0 ? y - 4 : 0;
            int rightLimit = y + 4 > 14 ? 14 : y + 4;

            int yl = leftLimit;
            int count = 0, Max = 0;

            while (yl <= rightLimit)
            {
                if (_stones[x, yl] != null && _stones[x, yl].Color == this.CurrentMoveColor)
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
                if (_stones[xl, b - xl] != null && _stones[xl, b - xl].Color == this.CurrentMoveColor)
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
                if (_stones[xl, b + xl] != null && _stones[xl, b + xl].Color == this.CurrentMoveColor)
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
                if (_stones[xl, y] != null && _stones[xl, y].Color == this.CurrentMoveColor)
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
                if (_stones[x, yl] != null && _stones[x, yl].Color == this.CurrentMoveColor)
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
    }

    public class FaultException : Exception
    {
        public FaultType Type { get; set; }

        public FaultException(FaultType fType)
        {
            this.Type = fType;
        }
    }

    public enum FaultType
    {
        double3,
        double4,
        over5,
    }

}
