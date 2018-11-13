using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.MatrixTable.Model
{
    public interface INeedMatrixClauses
    {
        INeedMatrixValues When(params IClause[] clauses);
    }

    public interface INeedMatrixValues
    {
        INeedMatrixClauses Then(params INodeValue[] values);
        Matrix End();
    }
    public class Matrix : INeedMatrixClauses, INeedMatrixValues
    {
        public List<Node> Nodes { get; private set; }
        public int NodeClauses { get; private set; }
        private IClause[] currentClauses;

        public Matrix()
        {
            NodeClauses = -1;
            Nodes = new List<Node>();
        }

        public INeedMatrixValues When(params IClause[] clauses)
        {
            if (clauses.Length < 1) throw new ApplicationException("The table must contain at least one column.");
            if (NodeClauses == -1)
            {
                NodeClauses = clauses.Length;
            }
            if (NodeClauses != clauses.Length)
            {
                throw new ApplicationException("Table column number is incorrect.");
            }

            currentClauses = clauses;
            return this;
        }

        public INeedMatrixClauses Then(params INodeValue[] values)
        {
            Nodes.Add(new Node(currentClauses, values));
            currentClauses = null;
            return this;
        }

        public Matrix End()
        {
            return this;
        }

        public object[] Calc(params object[] values)
        {
            if (values.Length != NodeClauses) throw new ApplicationException("The number of parameters does not match the number of columns in the table.");
            var found = Nodes.Find(n => Enumerable.Range(0, NodeClauses).All(i => n.Clauses[i].Match(values[i])));
            if (found != null) return PickValues(found.Values);
            return null;
        }

        public object[] PickValues(INodeValue[] values)
        {
            return values.Select(n => n.GetValue()).ToArray();
        }
    }
}
