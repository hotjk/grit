﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.Pattern.Flow
{
    public class Builder
    {
        public enum BuildState
        {
            Start = 0,
            When = 1,
            WhenAndThen = 2,
            Completed = 3,
        }

        private Builder() {}
        private BuildState buildState;
        private Transition buildTransition;
        private Instance instance;

        public static Builder Start(string name)
        {
            Builder builder = new Builder();
            builder.instance = new Instance(name);
            builder.buildState = BuildState.Start;
            return builder;
        }

        private void Append(IList<object> source, IEnumerable<object> states)
        {
            foreach (var state in states)
            {
                source.Add(state);
            }
        }

        public Builder When(IEnumerable<object> states)
        {
            if (buildState == BuildState.Start) // first transition
            {
                buildTransition = new Transition();
                Append(buildTransition.When, states);
                buildState = BuildState.When;
            }
            else if (buildState == BuildState.When)
            {
                throw new ApplicationException("Add WHEN for instance with invalid state.");
            }
            else if (buildState == BuildState.WhenAndThen) // close a transition and start a new transition
            {
                buildTransition.Assert();
                instance.AddTransition(buildTransition);

                buildTransition = new Transition();
                Append(buildTransition.When, states);
                buildState = BuildState.When;
            }
            else
            {
                throw new ApplicationException("Add WHEN for completed instance.");
            }
            return this;
        }

        public Builder When(params object[] states)
        {
            return this.When(states.AsEnumerable());
        }

        public Builder Then(IEnumerable<object> states)
        {
            if (buildState == BuildState.Start)
            {
                throw new ApplicationException("Add THEN without When.");
            }
            else if (buildState == BuildState.When) // add Then after When
            {
                Append(buildTransition.Then, states);
                buildState = BuildState.WhenAndThen;
            }
            else if (buildState == BuildState.WhenAndThen)
            {
                throw new ApplicationException("Add THEN for instance with invalid state.");
            }
            else
            {
                throw new ApplicationException("Add THEN for completed instance.");
            }
            return this;
        }

        public Builder Then(params object[] states)
        {
            return this.Then(states.AsEnumerable());
        }

        public Instance Complete()
        {
            if (buildState == BuildState.Start)
            {
                throw new ApplicationException("Complete instance without any transition.");
            }
            else if (buildState == BuildState.When) 
            {
                throw new ApplicationException("Complete instance without THEN.");
            }
            else if (buildState == BuildState.WhenAndThen)
            {
                buildTransition.Assert();
                instance.AddTransition(buildTransition);
                buildState = BuildState.Completed;
            }
            else
            {
                throw new ApplicationException("Instance already completed.");
            }

            instance.Completed();

            instance.Assert();
            return instance;
        }
    }
}
