using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public class DummyClause : IClause
    {
        public bool Match(object value)
        {
            return true;
        }

        public IEnumerable<IClause> GetClauses()
        {
            return Enumerable.Empty<IClause>();
        }

        public IEnumerable<OpBase> GetOps()
        {
            return Enumerable.Empty<OpBase>();
        }
    }
}
