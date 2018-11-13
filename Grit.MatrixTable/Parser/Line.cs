using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public class Line
    {
        public int Row { get; private set; }
        public string Text { get; private set; }

        public Line(string text)
        {
            Row = 0;
            Text = text;
        }

        public Line(int row, string text)
        {
            Row = row;
            Text = text;
        }
    }
}
