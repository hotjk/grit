using Grit.MatrixTable.Model;
using Grit.MatrixTable.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Builder
{
    public interface IMatrixNeedAxis
    {
        IMatrixNeedAxis String();
        IMatrixNeedAxis Int();
        IMatrixNeedAxis Float();
        IMatrixNeedAxis Double();
        IMatrixNeedAxis Decimal();
        IMatrixNeedAxis Column(IClauseParser clauseParser);
        IMatrixNeedValue Then();
    }
    public interface IMatrixNeedValue
    {
        IMatrixNeedValue String();
        IMatrixNeedValue Int();
        IMatrixNeedValue Float();
        IMatrixNeedValue Double();
        IMatrixNeedValue Decimal();
        IMatrixNeedValue Value(Func<string, object> getValue);
        MatrixParser Parser();
        Matrix Parse(string script);
    }

    public class MatrixBuilder : IMatrixNeedAxis, IMatrixNeedValue
    {
        private MatrixParser parser = new MatrixParser();
        private MatrixBuilder() { }
        public static IMatrixNeedAxis When()
        {
            MatrixBuilder builder = new MatrixBuilder();
            return builder;
        }
        IMatrixNeedAxis IMatrixNeedAxis.String()
        {
            parser.AddParam(ParserHelper.StringClause);
            return this;
        }
        IMatrixNeedAxis IMatrixNeedAxis.Int()
        {
            parser.AddParam(ParserHelper.IntClause);
            return this;
        }
        IMatrixNeedAxis IMatrixNeedAxis.Float()
        {
            parser.AddParam(ParserHelper.FloatClause);
            return this;
        }
        IMatrixNeedAxis IMatrixNeedAxis.Double()
        {
            parser.AddParam(ParserHelper.DoubleClause);
            return this;
        }
        IMatrixNeedAxis IMatrixNeedAxis.Decimal()
        {
            parser.AddParam(ParserHelper.DecimalClause);
            return this;
        }
        IMatrixNeedAxis IMatrixNeedAxis.Column(IClauseParser clauseParser)
        {
            parser.AddParam(clauseParser);
            return this;
        }
        IMatrixNeedValue IMatrixNeedAxis.Then()
        {
            return this;
        }

        IMatrixNeedValue IMatrixNeedValue.String()
        {
            parser.AddValue(ParserHelper.String);
            return this;
        }

        IMatrixNeedValue IMatrixNeedValue.Int()
        {
            parser.AddValue(ParserHelper.Int);
            return this;
        }

        IMatrixNeedValue IMatrixNeedValue.Float()
        {
            parser.AddValue(ParserHelper.Float);
            return this;
        }

        IMatrixNeedValue IMatrixNeedValue.Double()
        {
            parser.AddValue(ParserHelper.Double);
            return this;
        }

        IMatrixNeedValue IMatrixNeedValue.Decimal()
        {
            parser.AddValue(ParserHelper.Decimal);
            return this;
        }

        IMatrixNeedValue IMatrixNeedValue.Value(Func<string, object> getValue)
        {
            parser.AddValue(getValue);
            return this;
        }

        MatrixParser IMatrixNeedValue.Parser()
        {
            return parser;
        }

        Matrix IMatrixNeedValue.Parse(string script)
        {
            return parser.Parse(script);
        }
    }
}
