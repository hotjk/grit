using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class Instance
    {
        public Instance()
        {
            Transitions = new List<Transition>();
        }
        
        private IList<Transition> Transitions { get; set; }

        public void Add(Transition transition)
        {
            Transitions.Add(transition);
        }

        public void Assert()
        {
            // todo: find duplicate transition/cycle...
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var trans in Transitions)
            {
                sb.AppendLine(trans.ToString());
            }
            return sb.ToString();
        }
    }
}
