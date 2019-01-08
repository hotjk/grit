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
            sb.AppendLine(string.Join(",", flow.Nodes.Select(x => string.Format("{{ data: {{ id: '{0}' }} }}", x.Key))));
            sb.AppendLine("],");
            sb.AppendLine("edges: [");
            sb.AppendLine(string.Join(",", flow.Transitions.Select(x =>
            {
                StringBuilder buffer = new StringBuilder();
                foreach (var then in x.Then)
                {
                    var color = random.Next(0x1000000);
                    foreach (var when in x.When)
                    {
                        buffer.AppendFormat("{{ data: {{ source: '{0}', target: '{1}', faveColor: '{2}' }} }},", when, then, String.Format("#{0:X6}", color));
                    }
                }
                return buffer.ToString();
            })));
            sb.AppendLine("]");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
