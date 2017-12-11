using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            Part2,
            Part3,
            Part4,
            Part5,
            Part6,
            Part7,
            Part8,
            Part9
        }

        public enum Symbol
        {
            A,
            B,
            C,
            D,
            E,
            F
        }
        static void Main(string[] args)
        {
            var instance = Test1();

            string file = "./Web/code.js";
            string html = File.ReadAllText(file);
            File.WriteAllText(file, html.Replace("<@elements>", instance.CytoscapeJs()));
            System.Diagnostics.Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Web/demo.html"));
        }

        private static Instance Test1()
        {
            Builder.Start().Complete
            var instance = Builder.Start("Demo")
                .When(Steps.Part1).Then(Steps.Part2, Steps.Part3, Steps.Part4)
                .When(Steps.Part5, Steps.Part6).Then(Steps.Part8)
                .When(Steps.Part8).Then(Steps.Part9)
                .When(Steps.Part6, Steps.Part7).Then(Steps.Part8)
                .When(Steps.Part2, Steps.Part3, Steps.Part4).Then(Steps.Part5, Steps.Part6, Steps.Part7)
                .When(Steps.Part5, Steps.Part7).Then(Steps.Part8)
                .Complete();
            Console.WriteLine(instance);

            var target = instance.Next(Steps.Part1, Steps.Part2, Steps.Part3, Steps.Part4, Steps.Part6, Steps.Part7);
            Console.WriteLine(string.Join(", ", target));
            return instance;
        }
        private static Instance Test2()
        {
            var instance = Builder.Start("Demo")
                .When(Symbol.A).Then(Symbol.B)
                .When(Symbol.A, Symbol.B).Then(Symbol.C)
                .When(Symbol.A, Symbol.B, Symbol.C).Then(Symbol.D)
                .When(Symbol.B, Symbol.C, Symbol.D).Then(Symbol.E, Symbol.F)
                .Complete();
            Console.WriteLine(instance);

            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A, Symbol.B, Symbol.C)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A, Symbol.B, Symbol.C, Symbol.D)));
            return instance;
        }
        private static Instance Test3()
        {
            var instance = Builder.Start("Demo")
                .When(Symbol.A).Then(Symbol.B)
                .When(Symbol.A, Symbol.B).Then(Symbol.C)
                .Complete();
            Console.WriteLine(instance);

            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A, Symbol.B)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.B)));
            return instance;
        }
    }
}
