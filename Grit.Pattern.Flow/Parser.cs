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
            foreach (var trans in flow.Transitions)
            {
                sb.AppendFormat("{0} {2} {1}", string.Join(",", trans.When.OrderBy(x => x).ToArray()), string.Join(",", trans.Then.OrderBy(x => x).ToArray()), separator);
                sb.AppendLine();
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public static string Debug(IFlow flow)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Serialize(flow));
            foreach (var node in flow.Nodes.OrderBy(x => x.Weight))
            {
                sb.AppendFormat("{0} ({1}) -> {2}", node.Key, node.Weight, string.Join(", ", node.Target.Select(x => x.Key)));
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
