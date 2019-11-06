using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public static class CytoscapeJs
    {
        public static string JS(IFlow flow)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("elements: {");
            sb.AppendLine("nodes: [");
            foreach (var node in flow.Nodes())
            {
                sb.AppendFormat("{{ data: {{ id: '{0}' }} }}, ", node);
                sb.AppendLine();
            }
            sb.AppendLine("],");
            sb.AppendLine("edges: [");

            foreach(var then in flow.Thens())
            {
                var color = random.Next(0x1000000);
                foreach (var when in flow.When(then))
                {
                    sb.AppendFormat("{{ data: {{ source: '{0}', target: '{1}', faveColor: '{2}' }} }},", when, then, String.Format("#{0:X6}", color));
                    sb.AppendLine();
                }
            }
            
            sb.AppendLine("]");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
