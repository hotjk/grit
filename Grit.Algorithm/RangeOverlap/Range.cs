using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Algorithm.RangeOverlap
{
    public class Range<T> where T : IComparable<T>
    {
        public Range(T start, T end)
        {
            if(start.CompareTo(end) > 0)
            {
                throw new ArgumentException("End should large than start");
            }
            Start = start;
            End = end;
        }

        public T Start { get; private set; }
        public T End { get; private set; }

        public override string ToString()
        {
            return String.Format("({0},{1}]", Start, End);
        }

        public static bool IsOverlapping(Range<T> a, Range<T> b)
        {
            return (a.Start.CompareTo(b.End) <= 0 && a.End.CompareTo(b.Start) >= 0);
        }

        public static Range<T> Intersection(Range<T> a, Range<T> b)
        {
            if(!IsOverlapping(a,b))
            {
                return null;
            }
            return new Range<T>(a.Start.CompareTo(b.Start) < 0 ? a.Start : b.Start, 
                a.End.CompareTo(b.End) > 0 ? a.End : b.End);
        }

        public static Range<T> Intersection(IEnumerable<Range<T>> ranges)
        {
            if(ranges == null || !ranges.Any() ) { return null; }
            var first = new Range<T>(ranges.First().Start, ranges.First().End);

            if (ranges.Count() < 1) { return first; }
            var rest = ranges.Skip(1).ToList();

            while(rest.Any())
            {
                bool overlapping = false;
                for (var i =0;i< rest.Count; i++)
                {
                    var intersection = Range<T>.Intersection(first, rest[i]);
                    if (intersection != null)
                    {
                        first = intersection;
                        rest.RemoveAt(i);
                        overlapping = true;
                        break;
                    }
                }
                if(!overlapping)
                {
                    return null;
                }
            }
            return first;
        }
    }
}
