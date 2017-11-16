using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.Sequence
{
    public class SequenceRangeLoop
    {
        public SequenceRangeLoop(int id, int size)
        {
            Id = id;
            index = 0;

            loop = new List<SequenceRange>(size);
            for (int i = 0; i < size; i++)
            {
                loop[i] = new SequenceRange(id);
            }
        }
        public int Id { get; private set; }

        private int index;
        private IList<SequenceRange> loop;
        private object LockObject = new object();

        private void Turn()
        {
            index++;
            if (index == loop.Count)
            {
                index = 0;
            }
        }

        public int Next(int step, ISequenceRepository repository)
        {
            lock (LockObject)
            {
                foreach(var range in loop){
                    if(range.IsLoading)
                    {

                    }
                }
                Current = Current + 1;
                if (Current < this.Last)
                {
                    return Current;
                }

                Current = repository.NextWithTransactionScope(this.Id, step);
                if (Current < Last)
                {
                    throw new ApplicationException("The next value from SequenceRepository should large than orignal sequence value.");
                }
                Last = Current + step;
                return Current;
            }
        }
    }
}
