using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public interface IParser
    {
        void AddParam(IClauseParser clauseParser);
        void AddValue(Func<string, object> getV);
        Matrix Parse(IEnumerable<Line> lines);
    }
}
