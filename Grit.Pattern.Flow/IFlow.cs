using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public interface IFlow
    {
        IList<object> Next(params object[] source);
        IList<object> Next(IEnumerable<object> source);
        string Serialize();

        ISet<Node> Nodes { get; }
        IList<Transition> Transitions { get; }
    }
}
