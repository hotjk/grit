using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class CycleValidator
    {
        private Flow flow;
        private ISet<Node> whiteSet;
        private ISet<Node> graySet;
        private ISet<Node> blackSet;

        public CycleValidator(Flow flow)
        {
            this.flow = flow;
        }

        public bool HasCycle()
        {
            whiteSet = flow.Nodes;
            graySet = new HashSet<Node>();
            blackSet = new HashSet<Node>();

            while (whiteSet.Count > 0)
            {
                var node = whiteSet.First();
                if (Dfs(node)) { return true; }
            }
            return false;
        }

        private bool Dfs(Node node)
        {
            MoveNode(node, whiteSet, graySet);
            foreach (var neighbor in node.Target)
            {
                if (blackSet.Contains(neighbor)) { continue; }
                if (graySet.Contains(neighbor)) { return true; }
                if (Dfs(neighbor)) { return true; }
            }
            MoveNode(node, graySet, blackSet);
            return false;
        }

        private void MoveNode(Node node, ISet<Node> source, ISet<Node> target)
        {
            source.Remove(node);
            target.Add(node);
        }
    }
}
