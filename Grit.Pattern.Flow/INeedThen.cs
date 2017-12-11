using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public interface INeedThen
    {
        INeedWhen Then(IEnumerable<object> states);
        INeedWhen Then(params object[] states);
    }
}
