using Grit.MatrixTable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Parser
{
    public abstract class ValueParser
    {
        protected IList<Func<string, object>> GetValueFunctions;

        public ValueParser()
        {
            GetValueFunctions = new List<Func<string, object>>();
        }

        public void AddValue(Func<string, object> getV)
        {
            GetValueFunctions.Add(getV);
        }

        public INodeValue[] ParseValues(string strValues)
        {
            var values = strValues.Split3(ParserHelper.VALUE_SEP).ToArray();
            if (values.Length == 0) values = new string[] { string.Empty }; // for null value
            if (values.Length != GetValueFunctions.Count)
            {
                throw new ApplicationException("The values size should equals the value parser size.");
            }
            return values.Select((n, i) => ParserValue(n, GetValueFunctions[i])).ToArray();
        }

        private static INodeValue ParserValue(string value, Func<string, object> func)
        {
            value = value.Trim().RemoveEscape();
            if (value == string.Empty)
            {
                return new NodeNullValue();
            }
            return new NodeValue(func(value));
        }
    }
}
