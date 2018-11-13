using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public interface INodeValue
    {
        object GetValue();
    }

    public class NullValue
    {
        private static NullValue instance = new NullValue();
        public static NullValue Instance()
        {
            return instance;
        }
        private NullValue() { }
    }

    public class NodeNullValue : INodeValue
    {
        public object GetValue()
        {
            return NullValue.Instance();
        }
    }

    public class NodeValue : INodeValue
    {
        public object Value { get; private set; }

        public NodeValue(object value)
        {
            Value = value;
        }

        public object GetValue()
        {
            return Value;
        }
    }
}
