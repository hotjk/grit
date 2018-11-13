using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public abstract class CalculationStatement : IStatement
    {
        public object[] PickValues(INodeValue[] values)
        {
            return values.Select(n => n.GetValue()).ToArray();
        }

        public abstract object[] Calc(params object[] values);
    }
}
