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
            var instance = Test();

            string file = "./Web/code.js";
            string html = File.ReadAllText(file);
            File.WriteAllText(file, html.Replace("<@elements>", CytoscapeJs.JS(instance)));
            System.Diagnostics.Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Web/demo.html"));

            //string file = "./Web/code2.js";
            //string html = File.ReadAllText(file);
            //File.WriteAllText(file, html.Replace("<@elements>", CytoscapeJs.JS(instance)));
            //System.Diagnostics.Process.Start(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Web/demo2.html"));
        }
        private static IFlow Test()
        {
            var instance = Builder.Start(typeof(Steps))
                .When(Steps.Part1).Then(Steps.Part2, Steps.Part4)
                .When(Steps.Part2).Then(Steps.Part3)
                .When(Steps.Part3, Steps.Part5).Then(Steps.Part4)
                .When(Steps.Part4).Then(Steps.Part6)
                .Complete();
            Console.WriteLine(Parser.Serialize(instance));

            var target = instance.Next();
            Console.WriteLine(string.Join(", ", target)); // 1 5

            target = instance.Next(Steps.Part1);
            Console.WriteLine(string.Join(", ", target)); // 2 5

            target = instance.Next(Steps.Part1,Steps.Part2);
            Console.WriteLine(string.Join(", ", target)); // 3 5

            target = instance.Next(Steps.Part1, Steps.Part2, Steps.Part3);
            Console.WriteLine(string.Join(", ", target)); // 5

            target = instance.Next(Steps.Part1, Steps.Part2, Steps.Part3, Steps.Part5);
            Console.WriteLine(string.Join(", ", target)); // 4

            target = instance.Next(Steps.Part1, Steps.Part2, Steps.Part3, Steps.Part4, Steps.Part5);
            Console.WriteLine(string.Join(", ", target)); // 6

            target = instance.Next(Steps.Part1, Steps.Part2, Steps.Part3, Steps.Part4, Steps.Part5, Steps.Part6);
            Console.WriteLine(string.Join(", ", target)); // 
            return instance;
        }
        private static IFlow Test1()
        {
            var instance = Builder.Start(typeof(Steps))
                .When(Steps.Part1).Then(Steps.Part2, Steps.Part3, Steps.Part4)
                .When(Steps.Part5, Steps.Part6).Then(Steps.Part8)
                .When(Steps.Part8).Then(Steps.Part9)
                //.When(Steps.Part6, Steps.Part7).Then(Steps.Part8)
                .When(Steps.Part2, Steps.Part3, Steps.Part4).Then(Steps.Part5, Steps.Part6, Steps.Part7)
                .When(Steps.Part6, Steps.Part7).Then(Steps.Part9)
                .Complete();
            Console.WriteLine(Parser.Serialize(instance));

            var target = instance.Next(Steps.Part1, Steps.Part2, Steps.Part3, Steps.Part4, Steps.Part6, Steps.Part7);
            Console.WriteLine(string.Join(", ", target)); // Part5
            return instance;
        }
        private static IFlow Test2()
        {
            var instance = Builder.Start()
                .When(Symbol.A).Then(Symbol.B)
                .When(Symbol.A, Symbol.B).Then(Symbol.C)
                .When(Symbol.A, Symbol.B, Symbol.C).Then(Symbol.D)
                .When(Symbol.B, Symbol.C, Symbol.D).Then(Symbol.E, Symbol.F)
                .Complete();
            Console.WriteLine(Parser.Serialize(instance));

            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A, Symbol.B, Symbol.C)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A, Symbol.B, Symbol.C, Symbol.D)));
            return instance;
        }
        private static IFlow Test3()
        {
            var instance = Builder.Start()
                .When(Symbol.A).Then(Symbol.B)
                .When(Symbol.A, Symbol.B).Then(Symbol.C)
                .Complete();
            Console.WriteLine(Parser.Serialize(instance));

            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.A, Symbol.B)));
            Console.WriteLine(string.Join(", ", instance.Next(Symbol.B)));
            return instance;
        }

        private static IFlow Test4()
        {
            var instance = Builder.Start(typeof(Steps))
                .When(Steps.Part1).Then(Steps.Part2, Steps.Part3, Steps.Part4)
                .When(Steps.Part5, Steps.Part6).Then(Steps.Part8)
                .When(Steps.Part8).Then(Steps.Part9)
                .When(Steps.Part6, Steps.Part7).Then(Steps.Part8)
                .When(Steps.Part2, Steps.Part3, Steps.Part4).Then(Steps.Part5, Steps.Part6, Steps.Part7)
                .When(Steps.Part5, Steps.Part7).Then(Steps.Part8)
                .Complete();
            Console.WriteLine(Parser.Serialize(instance));

            var newInstance = (new Parser(typeof(Steps))).Parse(Parser.Serialize(instance));
            Console.WriteLine(Parser.Serialize(newInstance));
            return newInstance;
        }

        private static IFlow Test5()
        {
            var instance = Builder.Start(typeof(Steps))
                .When(Steps.Part1).Then(Steps.Part2, Steps.Part3, Steps.Part4)
                .When(Steps.Part5, Steps.Part6).Then(Steps.Part8)
                .When(Steps.Part8).Then(Steps.Part9)
                .When(Steps.Part6, Steps.Part7).Then(Steps.Part8)
                .When(Steps.Part2, Steps.Part3, Steps.Part4).Then(Steps.Part5, Steps.Part6, Steps.Part7)
                .When(Steps.Part5, Steps.Part7).Then(Steps.Part8)
                .When(Steps.Part9).Then(Steps.Part4)
                .When(Steps.Part9).Then(Steps.Part1)
                .Complete();
            Console.WriteLine(Parser.Serialize(instance));

            var newInstance = (new Parser(typeof(Steps))).Parse(Parser.Serialize(instance));
            Console.WriteLine(Parser.Serialize(newInstance));

            Console.WriteLine("Cycle? {0}", new CycleValidator(newInstance).HasCycle());
            return newInstance;
        }
    }
}
