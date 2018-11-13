using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public class Node
    {
        public IClause[] Clauses { get; private set; }
        public INodeValue[] Values { get; private set; }

        public Node(IClause[] clauses, INodeValue[] values)
        {
            Clauses = clauses;
            Values = values;
        }

        public Node(IClause column, IClause row, INodeValue[] values)
        {
            Clauses = new IClause[] { column, row };
            Values = values;
        }

        public Node(IClause clause, INodeValue[] values)
        {
            Clauses = new IClause[] { clause };
            Values = values;
        }

        public Node(INodeValue[] values)
        {
            Clauses = null;
            Values = values;
        }

        public IClause Column
        {
            get
            {
                return Clauses[0];
            }
        }

        public IClause Row
        {
            get
            {
                return Clauses[1];
            }
        }
    }
}
