using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Threading;
using System.Transactions;
using Grit.Sequence.Repository.MySql;

namespace Grit.Sequence.Demo
{
    class Program
    {
        public static readonly string Sequence = "Server=localhost;Port=3306;Database=grit;Uid=root;Pwd=flowers;";

        public static IList<int> TestResult;
        public static Random random = new Random();
        private const int SequenceID = 1;
        public static IKernel Kernel;
        static void Main(string[] args)
        {
            TestResult = new List<int>();
            AddIocBindings();

            //BasicTest();
            MultiThreadTest();
            //TransactionScopeTest();

            Console.WriteLine(TestResult.Count() == TestResult.Distinct().Count());
        }

        private static void AddIocBindings()
        {
            Kernel = new StandardKernel();
            SqlOption sqlOption = new SqlOption { ConnectionString = Sequence };
            Kernel.Bind<SqlOption>().ToConstant(sqlOption);
            Kernel.Bind<ISequenceRepository>().To<SequenceRepository>();
            Kernel.Bind<ISequenceService>().To<SequenceService>();
        }

        private static void MultiThreadTest()
        {
            
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < random.Next(20)+1; i++)
            {
                Thread thread = new Thread(BasicTest);
                thread.Name = i.ToString();
                threads.Add(thread);
            }
            threads.ForEach(x => x.Start());
            threads.ForEach(x => x.Join());
        }

        private static void BasicTest()
        {
            ISequenceService sequenceService = Kernel.Get<ISequenceService>();
            for (int i = 0; i < 100; i++)
            {
                int next = sequenceService.Next(SequenceID, 10);
                TestResult.Add(next);
                Console.Write(string.Format("{1}-{0}, ", Thread.CurrentThread.ManagedThreadId,next));
                Thread.Sleep(random.Next(100));
            }
        }

        private static void TransactionScopeTest()
        {
            ISequenceService sequenceService = Kernel.Get<ISequenceService>();
            using (TransactionScope scope = new TransactionScope())
            {
                BasicTest();
            }
        }
    }
}
