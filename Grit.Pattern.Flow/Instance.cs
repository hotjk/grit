using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class Instance
    {
        public Instance()
        {
            transitions = new List<Transition>();
            nodes = new List<Node>();
        }

        private IList<Transition> transitions;
        private IList<Node> nodes;

        private Node SetAndGet(object key)
        {
            var node = nodes.FirstOrDefault(x=>x.Key.Equals(key));
            if (node == null)
            {
                node = new Node(key);
                nodes.Add(node);
            }
            return node;
        }

        public void AddTransition(Transition transition)
        {
            transitions.Add(transition);

            foreach (var then in transition.Then)
            {
                SetAndGet(then);
            }

            foreach(var when in transition.When)
            {
                var nodeWhen = SetAndGet(when);
                foreach (var then in transition.Then)
                {
                    var nodeThen = SetAndGet(then);
                    nodeWhen.TryAdd(nodeThen);
                }
            }
        }

        public void Completed()
        {
            var roots = nodes.Where(node => !nodes.Any(x => x.Next.Any(n => n.Key.Equals(node.Key))));
            foreach(var node in roots)
            {
                CalculateWeight(node);
            }
        }

        private void CalculateWeight(Node node)
        {
            foreach(var next in node.Next)
            {
                next.Weight = next.Weight + node.Weight + 1;
                CalculateWeight(next);
            }
        }

        public void Assert()
        {
            // todo: find duplicate transition/cycle...
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var trans in transitions)
            {
                sb.AppendLine(trans.ToString());
            }

            foreach (var node in nodes)
            {
                sb.AppendLine(node.ToString());
            }
            return sb.ToString();
        }
    }
}
