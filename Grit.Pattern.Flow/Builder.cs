using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grit.Pattern.Flow
{
    public class Builder : INeedWhen, INeedThen, INeedScript
    {
        private Builder() {}
        private Transition buildTransition;
        private Instance instance;
        private Type type;

        public static INeedWhen Start(string name, Type type = null)
        {
            Builder builder = new Builder();
            builder.instance = new Instance(name);
            builder.type = type;
            return builder;
        }

        public static INeedScript Parser(string name, Type type)
        {
            Builder builder = new Builder();
            builder.instance = new Instance(name);
            builder.type = type;
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
            TypeCheck(states);
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

        private string separator = "->";
        private char[] stateSepaarator = new char[] { ',', ';', ' ' };
        private char[] lineSeparator = new char[] { '\r', '\n', ';' };

        public Instance Parse(string script)
        {
            if (type == null) throw new ApplicationException("Parsing from script requires state type.");
            
            var lines = script.Split(lineSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                int indexSeparator = line.IndexOf(separator);
                var left = line.Substring(0, indexSeparator).Trim();
                var right = line.Substring(indexSeparator + separator.Length).Trim();

                this.When(left.Split(stateSepaarator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x=>Enum.Parse(type, x)));

                this.Then(right.Split(stateSepaarator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x=>Enum.Parse(type, x)));
            }
            return this.Complete();
        }
    }
}
