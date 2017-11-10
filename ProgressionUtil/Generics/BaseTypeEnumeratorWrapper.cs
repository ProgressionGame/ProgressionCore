using System.Collections;
using System.Collections.Generic;

namespace Progression.Util.Generics
{
    public class BaseTypeEnumeratorWrapper<T, TBase> : IEnumerator<TBase> where T : TBase
    {
        private readonly IEnumerator<T> _enumeratorImplementation;
        public BaseTypeEnumeratorWrapper(IEnumerator<T> enumeratorImplementation)
        {
            _enumeratorImplementation = enumeratorImplementation;
        }
        public void Dispose() => _enumeratorImplementation.Dispose();

        public bool MoveNext() => _enumeratorImplementation.MoveNext();

        public void Reset() => _enumeratorImplementation.Reset();

        public T Current => _enumeratorImplementation.Current;
        TBase IEnumerator<TBase>.Current => _enumeratorImplementation.Current;
        object IEnumerator.Current => ((IEnumerator) _enumeratorImplementation).Current;
    }
}