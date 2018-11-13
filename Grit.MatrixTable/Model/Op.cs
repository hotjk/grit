using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.MatrixTable.Model
{
    public enum OpType
    {
        EQ,
        GE,
        GT,
        LE,
        LT,
        Contains,
        StartWith,
        EndWith
    }

    public abstract class OpBase
    {
        public OpType Type { get; protected set; }
        public abstract object GetValue();
    }

    public abstract class Op<T> : OpBase, IOp<T>
        where T : IEquatable<T>, IComparable<T>
    {
        public T Value { get; protected set; }

        public override object GetValue()
        {
            return Value;
        }

        public Op(OpType type, T value)
        {
            Type = type;
            Value = value;
        }

        public T GetInstanceValue()
        {
            return Value;
        }

        public abstract bool Match(T other);
    }

    public class Op4Number<T> : Op<T>
        where T : IEquatable<T>, IComparable<T>
    {
        public Op4Number(OpType type, T value) : base(type, value) { }

        public override bool Match(T other)
        {
            var v = GetInstanceValue();
            switch (Type)
            {
                case OpType.EQ:
                    return other.Equals(v);
                case OpType.GE:
                    return other.CompareTo(v) >= 0;
                case OpType.GT:
                    return other.CompareTo(v) > 0;
                case OpType.LE:
                    return other.CompareTo(v) <= 0;
                case OpType.LT:
                    return other.CompareTo(v) < 0;
            }
            throw new ApplicationException("Invalid operator type.");
        }
    }

    public class Op4String : Op<string>
    {
        public Op4String(OpType type, string value) : base(type, value) { }

        public override bool Match(string other)
        {
            var v = GetInstanceValue();
            switch (Type)
            {
                case OpType.EQ:
                    return other.Equals(v);
                case OpType.Contains:
                    return other.Contains(v);
                case OpType.StartWith:
                    return other.StartsWith(v);
                case OpType.EndWith:
                    return other.EndsWith(v);
            }
            throw new ApplicationException("Invalid operator type.");
        }
    }

    public class Op4Bool : Op<bool>
    {
        public Op4Bool(OpType type, bool value) : base(type, value) { }

        public override bool Match(bool other)
        {
            var v = GetInstanceValue();
            switch (Type)
            {
                case OpType.EQ:
                    return other.Equals(v);
            }
            throw new ApplicationException("Invalid operator type.");
        }
    }
}
