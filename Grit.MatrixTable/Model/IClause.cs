using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public interface IClause
    {
        bool Match(object value);
        IEnumerable<IClause> GetClauses();
        IEnumerable<OpBase> GetOps();
    }
}
