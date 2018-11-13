using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public abstract class ClauseParser<T> : IClauseParser
        where T : IComparable<T>, IEquatable<T>
    {
        protected Func<string, T> getT;
        public abstract IClause ParseClause(string left);

        public ClauseParser(Func<string, T> getT)
        {
            this.getT = getT;
        }
    }
}
