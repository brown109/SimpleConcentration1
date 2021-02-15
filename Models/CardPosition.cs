using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConcentration.Models
{
    //
    // structure to hold the row and column of the "card" that was clicked
    //
    public struct CardPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public CardPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
