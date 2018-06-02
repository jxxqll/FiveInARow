using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRow
{
    public class Stone
    {
        int _x;
        int _y;
        Board.StoneColor _color;

        public int X
        {
            get { return _x; }
            set { this._x = value; }
        }

        public int Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

        public Board.StoneColor Color
        {
            get { return this._color; }
            set { this._color = value; }
        }
    }

}
