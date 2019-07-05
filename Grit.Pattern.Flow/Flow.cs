using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class Flow : IFlow
    {
        internal Flow()
        {
            Transitions = new List<Transition>();
            nodes = new Dictionary<object, Node>();
        }

        public IList<Transition> Transitions { get; private set; }
        public IList<Node> Root { get; private set; }
        private IDictionary<object, Node> nodes;

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

        internal void AddTransition(Transition transition)
        {
            Transitions.Add(transition);

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

        internal void Completed()
        {
            Root = nodes.Values.Where(node => !nodes.Any(x => x.Value.Target.Contains(node))).ToList();
            if (!Root.Any())
            {
                throw new ApplicationException("There is not root node in the flow.");
            }
            if ((new CycleValidator(this)).HasCycle())
            {
                throw new ApplicationException("There may be cycle in the flow.");
            }
            foreach (var node in Root)
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
                    throw new ApplicationException("The flow is too big.");
                }
                CalculateWeight(next);
            }
        }

        public ISet<Node> Nodes
        {
            get
            {
                return new HashSet<Node>(nodes.Values);
                //return nodes.Values.ToHashSet();
            }
        }

        public IList<object> Next(params object[] source)
        {
            if(source.Length == 0)
            {
                return Root.Select(x=>x.Key).ToList();
            }
            return Next(source.AsEnumerable());
        }

        public IList<object> Next(IEnumerable<object> source)
        {
            if (source == null || !source.Any())
            {
                return Root.Select(x => x.Key).ToList();
            }

            var result = Transitions.Where(t => t.When.All(x => source.Any(n => n.Equals(x)))).SelectMany(t => t.Then).Distinct();
            if (result.Any()) 
            {
                int max = result.Max(x => nodes[x].Weight);
                result = result.Where(x => nodes[x].Weight == max);    
            }
            return result.ToList();
        }
    }
}
