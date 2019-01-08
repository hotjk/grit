using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class Node
    {
        public object Key { get; set; }
        public int Weight { get; set; }
        public ISet<Node> Target { get; set; }

        public Node(object key)
        {
            Key = key;
            this.Target = new HashSet<Node>();
        }

        public void TryAdd(Node node)
        {
            if (Target.Any(x => x.Key == node.Key)) return;
            Target.Add(node);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
