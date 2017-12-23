using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class Instance
    {
        public Instance(string name)
        {
            Name = name;
            transitions = new List<Transition>();
            nodes = new Dictionary<object, Node>();
        }

        public string Name { get; private set; }
        private IList<Transition> transitions;
        private IDictionary<object, Node> nodes;
        private IList<Node> root;

        private Node SetNode(object key)
        {
            Node node = null;
            if(!nodes.TryGetValue(key, out node))
            {
                node = new Node(key);
                nodes[key] = node;
            }
            return node;
        }

        public void AddTransition(Transition transition)
        {
            transitions.Add(transition);

            foreach (var then in transition.Then)
            {
                SetNode(then);
            }

            foreach (var when in transition.When)
            {
                var nodeWhen = SetNode(when);
                foreach (var then in transition.Then)
                {
                    nodeWhen.TryAdd(SetNode(then));
                }
            }
        }

        public void Completed()
        {
            root = nodes.Values.Where(node => !nodes.Any(x => x.Value.Target.Contains(node))).ToList();
            if (!root.Any())
            {
                throw new ApplicationException("There is not root node in the flow.");
            }
            foreach (var node in root)
            {
                CalculateWeight(node);
            }
        }

        private void CalculateWeight(Node node)
        {
            foreach (var next in node.Target)
            {
                next.Weight = next.Weight + node.Weight + 1;
                if (next.Weight > 100000)
                {
                    throw new ApplicationException("There may be loops in the flow.");
                }
                CalculateWeight(next);
            }
        }

        public IList<object> Next(params object[] source)
        {
            if(source.Length == 0)
            {
                return root.Select(x=>x.Key).ToList();
            }
            return Next(source.AsEnumerable());
        }

        public IList<object> Next(IEnumerable<object> source)
        {
            if (source == null || !source.Any())
            {
                return root.Select(x => x.Key).ToList();
            }

            var result = transitions.Where(t => t.When.All(x => source.Any(n => n.Equals(x)))).SelectMany(t => t.Then).Distinct();
            if (result.Any()) 
            {
                int max = result.Max(x => nodes[x].Weight);
                result = result.Where(x => nodes[x].Weight == max);    
            }
            return result.ToList();
        }

        public string CytoscapeJs()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("elements: {");
            sb.AppendLine("nodes: [");
            sb.AppendLine(string.Join(",", nodes.Select(x => string.Format("{{ data: {{ id: '{0}' }} }}", x.Key))));
            sb.AppendLine("],");
            sb.AppendLine("edges: [");
            sb.AppendLine(string.Join(",", transitions.Select(x =>
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            foreach (var trans in transitions)
            {
                sb.AppendLine(trans.ToString());
            }
            sb.AppendLine();
            foreach (var node in nodes.Values.OrderBy(x=>x.Weight))
            {
                sb.AppendLine(node.ToString());
            }
            return sb.ToString();
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var trans in transitions)
            {
                sb.AppendLine(trans.ToString());
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
