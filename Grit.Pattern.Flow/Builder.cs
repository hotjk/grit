using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.Pattern.Flow
{
    public class Builder : INeedWhen, INeedThen
    {
        private Transition building;
        private Flow instance;
        private Type type;

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

        private void Append(IList<object> source, IEnumerable<object> states)
        {
            foreach (var state in states)
            {
                source.Add(state);
            }
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
                building.Assert();
                instance.AddTransition(building);
            }
            building = new Transition();
            Append(building.When, states);

            return this;
        }

        public INeedThen When(params object[] states)
        {
            return this.When(states.AsEnumerable());
        }

        public INeedWhen Then(IEnumerable<object> states)
        {
            TypeCheck(states);
            Append(building.Then, states);
            return this;
        }

        public INeedWhen Then(params object[] states)
        {
            return this.Then(states.AsEnumerable());
        }

        public IFlow Complete()
        {
            if (building == null)
            {
                throw new ApplicationException("Complete a flow without any transition.");
            }
            building.Assert();
            instance.AddTransition(building);
            instance.Completed();
            return instance;
        }
    }
}
