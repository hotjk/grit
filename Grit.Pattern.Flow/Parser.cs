using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class Parser
    {
        private const string separator = "->";
        private readonly static char[] stateSeparator = new char[] { ',', ';', ' ' };
        private readonly static char[] lineSeparator = new char[] { '\r', '\n', ';' };
        private Type type;
        private Builder builder;

        public Parser(Type type)
        {
            this.type = type;
            this.builder = new Builder(type);
        }

        public IFlow Parse(string script)
        {
            var lines = script.Split(lineSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                int indexSeparator = line.IndexOf(separator);
                if (indexSeparator == -1)
                {
                    continue;
                }
                var left = line.Substring(0, indexSeparator).Trim();
                var right = line.Substring(indexSeparator + separator.Length).Trim();

                builder.When(left.Split(stateSeparator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Enum.Parse(type, x)));

                builder.Then(right.Split(stateSeparator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Enum.Parse(type, x)));
            }
            return builder.Complete();
        }

        public static string Serialize(IFlow flow)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var node in flow.Paths)
            {
                sb.AppendFormat("{0} -> {1}", node.Item1, node.Item2);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
