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
        bool _black;

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

        public bool Color
        {
            get { return this._black; }
            set { this._black = value; }
        }
    }

}
