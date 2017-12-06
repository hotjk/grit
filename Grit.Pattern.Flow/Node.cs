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
        public IList<Node> Next { get; set; }

        public Node(object key)
        {
            Key = key;
            this.Next = new List<Node>();
        }

        public void TryAdd(Node node)
        {
            if (Next.Any(x => x.Key == node.Key)) return;
            Next.Add(node);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]: {2}", Key, Weight, string.Join(", ", Next.Select(x => x.Key)));
        }
    }
}
