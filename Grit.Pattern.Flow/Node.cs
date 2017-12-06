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
        public IList<Node> Target { get; set; }

        public Node(object key)
        {
            Key = key;
            this.Target = new List<Node>();
        }

        public void TryAdd(Node node)
        {
            if (Target.Any(x => x.Key == node.Key)) return;
            Target.Add(node);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]: {2}", Key, Weight, string.Join(", ", Target.Select(x => x.Key)));
        }
    }
}
