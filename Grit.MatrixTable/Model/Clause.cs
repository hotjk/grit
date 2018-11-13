using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public class Clause<T> : IClause
        where T : IComparable<T>, IEquatable<T>
    {
        public IList<Op<T>> Ops { get; private set; }

        public Clause()
        {
            this.Ops = new List<Op<T>>();
        }

        public Clause<T> Op(Op<T> op)
        {
            Ops.Add(op);
            return this;
        }

        public bool Match(object value)
        {
            if (value is T)
            {
                return Match((T)value);
            }
            else
            {
                throw new ApplicationException("The clause type does not match the value type.");
            }
        }

        public virtual bool Match(T value)
        {
            return Ops.All(n => n.Match(value));
        }
    }
}
