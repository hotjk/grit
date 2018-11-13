using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public class NumberClauseParser<T> : ClauseParser<T> where T: IComparable<T>, IEquatable<T>
    {
        public NumberClauseParser(Func<string, T> getT) : base(getT) { }

        private Op4Number<T> ParseOp(OpType type, string value)
        {
            value = value.RemoveEscape();
            return new Op4Number<T>(type, getT(value));
        }

        public override IClause ParseClause(string left)
        {
            var clauses = new CompositeClause<T>();
            ParseClausePart(clauses, left);
            return clauses.Shrink() as Clause<T>;
        }

        private void ParseClausePart(CompositeClause<T> clauses, string left)
        {
            var clause = new Clause<T>();

            int indexNotation = left.IndexOf(ParserHelper.CLAUSE_SEP_CHA);
            if (indexNotation == -1)
            {
                if (left == string.Empty || left == ParserHelper.CLAUSE_EMPTY)
                {
                    clauses.AddClause(clause); // ()
                    return;
                }
                clauses.AddClause(clause.Op(ParseOp(OpType.EQ, left))); // 20
                return;
            }

            var strLower = left.Substring(0, indexNotation).Trim(); // [10 of [10,20),25,(30,]
            var strRemains = left.Substring(indexNotation + 1).Trim(); // ,20),25,(30,] of [10,20),25,(30,]
            if (strLower[0] != ParserHelper.CLAUSE_GE && strLower[0] != ParserHelper.CLAUSE_GT) // 25 of [10,20),25,(30,]
            {
                clauses.AddClause(clause.Op(ParseOp(OpType.EQ, strLower)));
            }
            else
            {
                var nextNotation = strRemains.IndexOf(ParserHelper.CLAUSE_SEP_CHA);
                var strUpper = strRemains;
                if (nextNotation != -1)
                {
                    strUpper = strRemains.Substring(0, nextNotation).Trim(); // 25,(30,] of [10,20),25,(30,]
                    strRemains = strRemains.Substring(nextNotation + 1).Trim(); // 20) of [10,20),25,(30,]
                }
                else
                {
                    strRemains = null;
                }

                if (strLower.Length > 1)
                {
                    clause.Op(ParseOp((strLower[0] == ParserHelper.CLAUSE_GE) ? OpType.GE : OpType.GT, strLower.Substring(1)));
                }
                if (strUpper.Length > 1)
                {
                    clause.Op(ParseOp((strUpper[strUpper.Length - 1] == ParserHelper.CLAUSE_LE) ? OpType.LE : OpType.LT, strUpper.Substring(0, strUpper.Length - 1)));
                }
                clauses.AddClause(clause);
            }
            
            if(!string.IsNullOrEmpty(strRemains))
            {
                ParseClausePart(clauses, strRemains);
            }
            return;
        }
    }
}
