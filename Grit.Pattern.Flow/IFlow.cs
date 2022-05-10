using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public interface IFlow
    {
        List<Tuple<object, object>> Paths { get; }
        IList<object> Next(params object[] source);

        IEnumerable<object> Nodes();
        IEnumerable<object> When();
        IEnumerable<object> Then();
        IEnumerable<object> When(object then);
        IEnumerable<object> Then(object when);
    }
}
