using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.Sequence
{
    public class SequenceRange
    {
        public SequenceRange(int id)
        {
            this.Id = id;
            this.Current = 0;

            Slices = new Queue<SequenceSlice>();
        }

        public int Id { get; private set; }
        public int Current { get; private set; }

        public Queue<SequenceSlice> Slices { get; private set; }

        public object LockRange = new object();
        public object LockSlice = new object();

        public int Next(int step, ISequenceRepository repository)
        {
            lock (LockRange)
            {
                SequenceSlice slice;
                if(Slices.Count == 0)
                {
                    Loading(step, repository);
                    slice = Slices.First();
                    Current = slice.From;
                }

                slice = Slices.First();
                if (Current == slice.Middle)
                {
                    Task.Run(() =>
                    {
                        Loading(step, repository);
                    });
                }
                Current = Current + 1;

                if(Current == slice.To)
                {
                    lock(LockSlice)
                    {
                        Slices.Dequeue();
                    }
                    while(Slices.Count == 0)
                    {
                        Thread.Sleep(0);
                    }
                    slice = Slices.First();
                    Current = slice.From;
                }
                return Current;
            }
        }

        private void Loading(int step, ISequenceRepository repository)
        {
            int from = repository.NextWithTransactionScope(this.Id, step);
            lock (LockSlice)
            {
                Slices.Enqueue(new SequenceSlice(from, from + step));
                //Console.WriteLine(this);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{2}## Sequence: {0}, {1}", this.Id, this.Current, Environment.NewLine);
            foreach(var slice in this.Slices)
            {
                sb.AppendFormat(", {0}", slice);
            }
            return sb.ToString();
        }
    }
}
