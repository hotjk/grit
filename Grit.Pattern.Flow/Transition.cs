using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class Transition
    {
        public Transition()
        {
            this.When = new List<object>();
            this.Then = new List<object>();
        }
        public IList<object> When { get; set; }
        public IList<object> Then { get; set; }

        public void Assert()
        {
            if (!When.Any() || !Then.Any())
            {
                throw new ApplicationException("When or Then should not be empty.");
            }
            if (When.Distinct().Count() != When.Count)
            {
                throw new ApplicationException("Duplicate When.");
            }
            if (Then.Distinct().Count() != Then.Count)
            {
                throw new ApplicationException("Duplicate Then.");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", string.Join(",", When.OrderBy(x => x).ToArray()), string.Join(",", Then.OrderBy(x => x).ToArray()));
        }
    }
}
