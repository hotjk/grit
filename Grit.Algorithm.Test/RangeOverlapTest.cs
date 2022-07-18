using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grit.Algorithm.RangeOverlap;

namespace Grit.Algorithm.Test
{
    public static class RangeOverlapTest
    {
        public static void Test()
        {
            IsOverlapping(new Range<int>(5, 10), new Range<int>(8, 12));
            IsOverlapping(new Range<int>(5, 10), new Range<int>(10, 12));
            IsOverlapping(new Range<int>(5, 10), new Range<int>(11, 12)); //f
            IsOverlapping(new Range<int>(5, 10), new Range<int>(0, 12));
            IsOverlapping(new Range<int>(5, 10), new Range<int>(0, 4));   //f
            IsOverlapping(new Range<int>(5, 10), new Range<int>(0, 5));

            IsOverlapping(new Range<DateTime>(new DateTime(2000, 1, 1), new DateTime(2000, 2, 1)),
                new Range<DateTime>(new DateTime(2000, 2, 1), new DateTime(2000, 3, 1)));
            IsOverlapping(new Range<DateTime>(new DateTime(2000, 1, 1), new DateTime(2000, 2, 1)),
                new Range<DateTime>(new DateTime(2000, 1, 31), new DateTime(2000, 3, 1)));
            IsOverlapping(new Range<DateTime>(new DateTime(2000, 1, 1), new DateTime(2000, 2, 1)),
                new Range<DateTime>(new DateTime(2000, 2, 28), new DateTime(2000, 3, 1))); // f

            Intersection(new Range<int>(5, 10), new Range<int>(8, 12));
            Intersection(new Range<int>(5, 10), new Range<int>(0, 4));
            Intersection(new Range<int>(5, 10), new Range<int>(0, 5));

            Intersection(new Range<int>(5, 10), new Range<int>(8, 12), new Range<int>(11, 18));
            Intersection(new Range<int>(5, 6), new Range<int>(8, 12), new Range<int>(11, 18));
            Intersection(new Range<int>(5, 6), new Range<int>(8, 12), new Range<int>(6, 18));
            Intersection(new Range<int>(5, 6), new Range<int>(8, 12), new Range<int>(6, 18), new Range<int>(22, 30));
        }

        private static void IsOverlapping<T>(Range<T> start, Range<T> end) where T : IComparable<T>
        {
            bool result = Range<T>.IsOverlapping(start, end);
            Console.WriteLine("{0}, {1} is overlapping: {2}", start, end, result);
        }

        public static void Intersection<T>(Range<T> start, Range<T> end) where T : IComparable<T>
        {
            var result = Range<T>.Intersection(start, end);
            Console.WriteLine("{0}, {1} Intersection: {2}", start, end, result);
        }

        public static void Intersection<T>(params Range<T>[] ranges) where T : IComparable<T>
        {
            var result = Range<T>.Intersection(ranges);
            Console.WriteLine("{0} Intersection: {1}", string.Join(", ", ranges.Select(x => x.ToString())), result);
        }
    }
}
