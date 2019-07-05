using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public static class ParserHelper
    {
        public readonly static Func<string, object> Int = (x => int.Parse(x.Trim()));
        public readonly static Func<string, object> String = (x => x.Trim());
        public readonly static Func<string, object> Float = (x => float.Parse(x.Trim()));
        public readonly static Func<string, object> Double = (x => double.Parse(x.Trim()));
        public readonly static Func<string, object> Decimal = (x => decimal.Parse(x.Trim()));
        public readonly static Func<string, object> Bool = (x => bool.Parse(x.Trim()));
        public readonly static Func<string, object> Date = (x => DateTime.Parse(x.Trim()));
        public readonly static Func<string, object> Dummy = (x => null);

        public readonly static StringClauseParser StringClause = new StringClauseParser(x => x.Trim());
        public readonly static NumberClauseParser<int> IntClause = new NumberClauseParser<int>(x => int.Parse(x.Trim()));
        public readonly static NumberClauseParser<float> FloatClause = new NumberClauseParser<float>(x => float.Parse(x.Trim()));
        public readonly static NumberClauseParser<double> DoubleClause = new NumberClauseParser<double>(x => double.Parse(x.Trim()));
        public readonly static NumberClauseParser<decimal> DecimalClause = new NumberClauseParser<decimal>(x => decimal.Parse(x.Trim()));
        public readonly static BoolClauseParser BoolClause = new BoolClauseParser(x => bool.Parse(x.Trim()));
        public readonly static NumberClauseParser<DateTime> DateClause = new NumberClauseParser<DateTime>(x => DateTime.Parse(x.Trim()));
        public readonly static DummyClauseParser DummyClause = new DummyClauseParser();

        public readonly static char ESCAPE = '\\';
        public readonly static char[] CLAUSE_VALUE_SEP = new char[] { '=', '|' };
        public readonly static char[] TREE_CLAUSE_VALUE_SEP = new char[] { '=' };
        public readonly static char VARIABLE_CLAUSE_SEP = '=';
        public readonly static char[] LINE_SEP = new char[] { '\r', '\n', ';' };
        public readonly static char[] VALUE_SEP = new char[] { ',', '/' };
        public readonly static char[] CLAUSE_SEP = new char[] { '|' };
        public readonly static char[] EXCLUDE = new char[] { '\'', '"' };
        public readonly static char REF_VALUE_PRE = '[';
        public readonly static char REF_VALUE_POST = ']';
        public readonly static char EXPR_PRE = '{';
        public readonly static char EXPR_POST = '}';
        public readonly static char LINK_PRE = '<';
        public readonly static char LINK_POST = '>';
        public readonly static char REF_CLAUSE_PRE = '@';
        public readonly static char CONTINUATION = '\\';
        public readonly static char LOCAL_SCOPE = '#';
        public readonly static char CLAUSE_GE = '[';
        public readonly static char CLAUSE_LE = ']';
        public readonly static char CLAUSE_GT = '(';
        public readonly static char CLAUSE_LT = ')';
        public readonly static string CLAUSE_EMPTY = "()";
        public readonly static char CLAUSE_SEP_CHA = ',';
        public readonly static char[] CLAUSE_SEP_ARR = new char[] { CLAUSE_SEP_CHA };
        public readonly static string CLAUSE_SEP_STR = string.Format("{0} ", CLAUSE_SEP_CHA);
        public readonly static string CLAUSE_ANY = "*";
        public readonly static char CLAUSE_LIKE = '%';
        public readonly static char CLAUSE_REGEX = '~';

        public readonly static string CSS_PARAMETER = "yep-parameter";
        public readonly static string CSS_RETURN_VALUE = "yep-returnvalue";
        public readonly static string CSS_REFERENCE = "yep-reference";
        public readonly static string CSS_CLAUSE = "yep-clause";
        public readonly static string CSS_VALUE = "yep-value";
        public readonly static string CSS_LINK = "yep-link";

        public static IEnumerable<string> Split3(this string str, char[] sep, bool removeEmpty = false)
        {
            if (string.IsNullOrEmpty(str)) yield break;

            int idxFrom = 0;
            bool idxExscape = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (idxExscape)
                {
                    idxExscape = false;
                    continue;
                }
                if (ESCAPE == str[i])
                {
                    idxExscape = true;
                    continue;
                }
                if (sep.Contains(str[i]))
                {
                    if (i >= idxFrom)
                    {
                        var sub = str.Substring(idxFrom, i - idxFrom).Trim();
                        if (!removeEmpty || !string.IsNullOrEmpty(sub)) yield return sub;
                    }
                    idxFrom = i + 1;
                }
            }

            if (idxExscape) throw new ApplicationException("Split find escape symbol is orphan. -- " + str);
            var subLast = str.Substring(idxFrom).Trim();

            if (!removeEmpty || !string.IsNullOrEmpty(subLast)) yield return subLast;
        }

        public static string RemoveEscape(this string str)
        {
            return str.Replace(ESCAPE.ToString(), string.Empty);
        }

        public static string Escape(this string str, char[] sep)
        {
            foreach (var c in sep)
            {
                var s = c.ToString();
                str = str.Replace(s, ESCAPE + s);
            }
            return str;
        }
    }

    public static class ParserExtension
    {
        public static IEnumerable<Line> SkipComments(this IEnumerable<Line> lines)
        {
            return lines
                .Select((n, index) =>
                {
                    int i = n.Text.IndexOf("--");
                    int j = n.Text.IndexOf("//");
                    if (j != -1 && j < i) i = j; // find first comments symbol
                    if (i == -1)
                    {
                        return new Line(index, n.Text);
                    }
                    return new Line(index, n.Text.Substring(0, i));
                })
                .Where(n => !string.IsNullOrWhiteSpace(n.Text));
        }

        public static IEnumerable<Line> LinesWithRowNumber(this string lines)
        {
            var ArrLines = lines.Split(ParserHelper.LINE_SEP, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < ArrLines.Count; i++)
            {
                var line = ArrLines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line[line.Length - 1] == ParserHelper.CONTINUATION && i < ArrLines.Count - 1)
                {
                    ArrLines[i] = line.Substring(0, line.Length - 1) + ArrLines[i + 1];
                    ArrLines.RemoveAt(i + 1);
                    i--;
                }
            }

            return ArrLines.Select((n, i) => new Line(i, n)).SkipComments();
        }
    }
}
