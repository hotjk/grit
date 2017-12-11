using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public interface INeedWhen
    {
        INeedThen When(IEnumerable<object> states);
        INeedThen When(params object[] states);
        Instance Complete();
    }
}
