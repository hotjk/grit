using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public class ParseException : ApplicationException
    {
        public ParseException(string message)
            : base(message)
        {
        }

        public ParseException(string message, Line line)
            : base(string.Format("{0}{1}line: {2} -- {3}", message, Environment.NewLine, line.Row, line.Text))
        {
        }
    }
}
