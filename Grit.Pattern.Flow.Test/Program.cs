using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Pattern.Flow.Test
{
    class Program
    {
        public enum Steps
        {
            Part1,
            Part2_1,
            Part2_2,
            Part2_3,
            Part3_1,
            Part3_2,
            Part3_3,
            Part4,
            Part5
        }
        static void Main(string[] args)
        {
            var instance = Builder.Start()
                .When(Steps.Part1).Then(Steps.Part2_1, Steps.Part2_2, Steps.Part2_3)
                .When(Steps.Part3_1, Steps.Part3_2).Then(Steps.Part4)
                .When(Steps.Part4).Then(Steps.Part5)
                .When(Steps.Part3_2, Steps.Part3_3).Then(Steps.Part4)
                .When(Steps.Part2_1, Steps.Part2_2, Steps.Part2_3).Then(Steps.Part3_1, Steps.Part3_2, Steps.Part3_3)
                .When(Steps.Part3_1, Steps.Part3_3).Then(Steps.Part4)
                .Complete();
            Console.WriteLine(instance);
        }
    }
}
