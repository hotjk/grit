using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public class StringClauseParser : ClauseParser<string>
    {
        public StringClauseParser(Func<string, string> stringParser) : base(stringParser) { }

        private Op4String ParseOp(OpType type, string value)
        {
            value = value.RemoveEscape();
            return new Op4String(type, getT(value));
        }

        public override IClause ParseClause(string left)
        {
            var values = left.Split3(ParserHelper.CLAUSE_SEP_ARR).ToArray();
            var clauses = new CompositeClause<string>();
            ParseClausePart(clauses, values);
            return clauses.Shrink() as Clause<string>;
        }

        private void ParseClausePart(CompositeClause<string> clauses, IEnumerable<string> values)
        {
            var clause = new Clause<string>();

            if (!values.Any())
            {
                clauses.AddClause(clause); // ()=0
            }
            else
            {
                string left = values.First();
                if (left == ParserHelper.CLAUSE_ANY) { }
                else if (left[0] == ParserHelper.CLAUSE_LIKE)
                {
                    left = left.Substring(1);
                    if (left[left.Length - 1] == ParserHelper.CLAUSE_LIKE)
                    {
                        clauses.AddClause(clause.Op(ParseOp(OpType.Contains, left.Substring(0, left.Length - 1))));
                    }
                    else
                    {
                        clauses.AddClause(clause.Op(ParseOp(OpType.EndWith, left)));
                    }
                }
                else
                {
                    if (left[left.Length - 1] == ParserHelper.CLAUSE_LIKE)
                    {
                        clauses.AddClause(clause.Op(ParseOp(OpType.StartWith, left.Substring(0, left.Length - 1))));
                    }
                    else
                    {
                        clauses.AddClause(clause.Op(ParseOp(OpType.EQ, left)));
                    }
                }
            }

            if(values.Skip(1).Any())
            {
                ParseClausePart(clauses, values.Skip(1));
            }
        }
    }
}
