using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.Pattern.Flow
{
    public class Builder : INeedWhen, INeedThen
    {
        private Tuple<HashSet<object>, HashSet<object>> building;
        private readonly Flow instance;
        private readonly Type type;

        internal Builder(Type type = null)
        {
            this.instance = new Flow();
            this.type = type;
        }

        public static INeedWhen Start(Type type = null)
        {
            Builder builder = new Builder(type);
            return builder;
        }

        private void TypeCheck(IEnumerable<object> states)
        {
            if (type == null) return;
            foreach (var state in states)
            {
                if (state.GetType() != type)
                {
                    throw new ApplicationException("Invalid state.");
                }
            }
        }

        public INeedThen When(IEnumerable<object> states)
        {
            TypeCheck(states);
            if (building != null)
            {
                instance.AddTransition(building);
            }
            building = new Tuple<HashSet<object>, HashSet<object>>(new HashSet<object>(), new HashSet<object>());
            building.Item1.UnionWith(states);
            return this;
        }

        public INeedThen When(params object[] states)
        {
            return this.When(states.AsEnumerable());
        }

        public INeedWhen Then(IEnumerable<object> states)
        {
            TypeCheck(states);
            building.Item2.UnionWith(states);
            return this;
        }

        public INeedWhen Then(params object[] states)
        {
            return this.Then(states.AsEnumerable());
        }

        public IFlow Complete()
        {
            if (building != null)
            {
                instance.AddTransition(building);
            }
            return instance;
        }
    }
}
