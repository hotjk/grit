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
            Paths = new List<Tuple<object, object>>();
        }

        public List<Tuple<object, object>> Paths { get; private set; }

        internal void AddTransition(Tuple<HashSet<object>, HashSet<object>> transition)
        {
            foreach (var when in transition.Item1)
            {
                foreach(var then in transition.Item2)
                {
                    Paths.Add(new Tuple<object, object>(when, then));
                }
            }
        }

        public IList<object> Next(params object[] source)
        {
            return Next(source.AsEnumerable());
        }

        private IList<object> Next(IEnumerable<object> source)
        {
            var nodes = Enumerable.Except(Nodes(), source).ToList();
            return nodes.Where(then => When(then).All(x => source.Contains(x))).ToList();
        }

        public IEnumerable<object> Nodes()
        {
            return Paths.Select(x => x.Item1).Union(Paths.Select(x => x.Item2).Distinct());
        }

        public IEnumerable<object> Whens()
        {
            return Paths.Select(x => x.Item1).Distinct();
        }

        public IEnumerable<object> Thens()
        {
            return Paths.Select(x => x.Item2).Distinct();
        }

        public IEnumerable<object> When(object then)
        {
            return Paths.Where(x => x.Item2.Equals(then)).Select(x => x.Item1).Distinct();
        }

        public IEnumerable<object> Then(object when)
        {
            return Paths.Where(x => x.Item1.Equals(when)).Select(x => x.Item2).Distinct();
        }
    }
}
