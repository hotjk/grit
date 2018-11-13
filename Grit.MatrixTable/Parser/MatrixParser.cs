using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public class MatrixParser : ValueParser, IParser
    {
        private List<IClauseParser> columnParsers;
        public void AddParam(IClauseParser clauseParser)
        {
            columnParsers.Add(clauseParser);
        }
        public MatrixParser(params IClauseParser[] columnParsers)
        {
            this.columnParsers = new List<IClauseParser>();
            this.columnParsers.AddRange(columnParsers);
        }

        public Matrix Parse(string script)
        {
            return Parse(script.LinesWithRowNumber());
        }

        public Matrix Parse(IEnumerable<Line> lines)
        {
            if (!columnParsers.Any()) throw new ApplicationException("The matrix requires at least one column.");
            if (!GetValueFunctions.Any()) throw new ApplicationException("The matrix requires a function to parse value.");

            if (!lines.Any()) throw new ApplicationException("The matrix requires at least one row.");

            Matrix matrix = new Matrix();

            foreach (var line in lines)
            {
                try
                {
                    var strColumns = line.Text.Split3(ParserHelper.CLAUSE_SEP).ToArray();
                    if (strColumns.Length < 2) throw new ApplicationException("The matrix requires at least one clause and one value.");

                    matrix.When(strColumns.Take(strColumns.Length - 1).Select((x, i) => columnParsers[i].ParseClause(x)).ToArray())
                        .Then(ParseValues(strColumns[strColumns.Length - 1]));
                }
                catch (Exception ex)
                {
                    throw new ParseException(ex.Message, line);
                }
            }
            return matrix;
        }

        Matrix IParser.Parse(IEnumerable<Line> lines)
        {
            return Parse(lines);
        }
    }
}
