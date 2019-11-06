using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow
{
    public class CycleValidator
    {
        private readonly IFlow flow;
        private ISet<object> whiteSet;
        private ISet<object> graySet;
        private ISet<object> blackSet;

        public CycleValidator(IFlow flow)
        {
            this.flow = flow;
        }

        public bool HasCycle()
        {
            whiteSet = new HashSet<object>(flow.Nodes());
            graySet = new HashSet<object>();
            blackSet = new HashSet<object>();

            while (whiteSet.Count > 0)
            {
                var node = whiteSet.First();
                if (Dfs(node)) { return true; }
            }
            return false;
        }

        private bool Dfs(object node)
        {
            MoveNode(node, whiteSet, graySet);
            foreach (var neighbor in flow.Then(node))
            {
                if (blackSet.Contains(neighbor)) { continue; }
                if (graySet.Contains(neighbor)) { return true; }
                if (Dfs(neighbor)) { return true; }
            }
            MoveNode(node, graySet, blackSet);
            return false;
        }

        private void MoveNode(object node, ISet<object> source, ISet<object> target)
        {
            source.Remove(node);
            target.Add(node);
        }
    }
}
