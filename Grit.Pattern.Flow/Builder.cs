using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.Pattern.Flow
{
    public class Builder : INeedWhen, INeedThen
    {
        private Builder() {}
        private Transition buildTransition;
        private Instance instance;

        public static INeedWhen Start(string name)
        {
            Builder builder = new Builder();
            builder.instance = new Instance(name);
            return builder;
        }

        private void Append(IList<object> source, IEnumerable<object> states)
        {
            foreach (var state in states)
            {
                source.Add(state);
            }
        }

        public INeedThen When(IEnumerable<object> states)
        {
            if (buildTransition != null)
            {
                buildTransition.Assert();
                instance.AddTransition(buildTransition);
            }
            buildTransition = new Transition();
            Append(buildTransition.When, states);

            return this;
        }

        public INeedThen When(params object[] states)
        {
            return this.When(states.AsEnumerable());
        }

        public INeedWhen Then(IEnumerable<object> states)
        {
            Append(buildTransition.Then, states);
            return this;
        }

        public INeedWhen Then(params object[] states)
        {
            return this.Then(states.AsEnumerable());
        }

        public Instance Complete()
        {
            if (buildTransition == null)
            {
                throw new ApplicationException("Complete instance without any transition.");
            }
            buildTransition.Assert();
            instance.AddTransition(buildTransition);
            instance.Completed();
            return instance;
        }
    }
}
