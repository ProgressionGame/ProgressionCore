using System.Collections;
using System.Collections.Generic;

namespace Progression
{
    public class SingleItemEnumerator<T> : IEnumerator<T>
    {
        public SingleItemEnumerator(T item)
        {
            Item = item;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (State) return false;
            State = true;
            return true;
        }

        public void Reset()
        {
            State = false;
        }

        public T Item { get; }
        public bool State { get; private set; }
        public T Current => State ? Item : default(T);

        object IEnumerator.Current => Current;
    }
}