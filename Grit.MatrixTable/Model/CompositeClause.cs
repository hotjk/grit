using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public class CompositeClause<T> : Clause<T> where T : IComparable<T>, IEquatable<T>
    {
        public IList<Clause<T>> Clauses { get; private set; }

        public CompositeClause()
        {
            Clauses = new List<Clause<T>>();
        }

        public void AddClause(Clause<T> clause)
        {
            Clauses.Add(clause);
        }

        public IClause Shrink()
        {
            return Clauses.Count == 1 ? Clauses[0] : this;
        }

        public override bool Match(T value)
        {
            if (!Clauses.Any()) return true;
            return Clauses.Any(n => n.Match(value));
        }
    }
}
