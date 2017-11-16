using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Sequence
{
    public class SequenceSlice
    {
        public SequenceSlice(int from, int to)
        {
            if(to < from)
            {
                throw new ArgumentException("Invalid sequence slice.");
            }
            this.From = from;
            this.To = to;
        }
        public int From { get; private set; }
        public int To { get; private set; }

        public int Middle
        {
            get
            {
                return (To - From) / 2 + From;
            }
        }

        public override string ToString()
        {
            return string.Format("From: {0}, To: {1}", From, To);
        }
    }
}
