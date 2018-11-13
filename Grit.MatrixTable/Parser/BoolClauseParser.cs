using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public class BoolClauseParser : ClauseParser<bool>
    {
        public BoolClauseParser(Func<string, bool> boolParser) : base(boolParser) { }

        private Op4Bool ParseOp(OpType type, string value)
        {
            value = value.RemoveEscape();
            return new Op4Bool(type, getT(value));
        }

        public override IClause ParseClause(string left)
        {
            var values = left.Split3(ParserHelper.CLAUSE_SEP_ARR).ToArray();
            var clauses = new CompositeClause<bool>();
            ParseClausePart(clauses, values);
            return clauses.Shrink() as Clause<bool>;
        }

        private void ParseClausePart(CompositeClause<bool> clauses, IEnumerable<string> values)
        {
            var clause = new Clause<bool>();

            if (!values.Any())
            {
                clauses.AddClause(clause); // ()=0
            }
            else
            {
                string left = values.First();
                if (left == ParserHelper.CLAUSE_ANY) { }
                else
                {
                    clauses.AddClause(clause.Op(ParseOp(OpType.EQ, left)));
                }
            }
            if(values.Skip(1).Any())
            {
                ParseClausePart(clauses, values.Skip(1));
            }
        }
    }
}
