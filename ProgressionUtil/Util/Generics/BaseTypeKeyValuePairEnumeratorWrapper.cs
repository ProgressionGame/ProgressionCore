using System.Collections;
using System.Collections.Generic;

namespace Progression.Util.Generics
{
    public class BaseTypeKeyValuePairEnumeratorWrapper<TKey, TValue, TValueBase> : IEnumerator<KeyValuePair<TKey, TValueBase>> where TValue : TValueBase
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> _enumeratorImplementation;
        public BaseTypeKeyValuePairEnumeratorWrapper(IEnumerator<KeyValuePair<TKey, TValue>> enumeratorImplementation)
        {
            _enumeratorImplementation = enumeratorImplementation;
        }
        public void Dispose() => _enumeratorImplementation.Dispose();

        public bool MoveNext() => _enumeratorImplementation.MoveNext();

        public void Reset() => _enumeratorImplementation.Reset();

        public KeyValuePair<TKey, TValue> Current => _enumeratorImplementation.Current;
        KeyValuePair<TKey, TValueBase> IEnumerator<KeyValuePair<TKey, TValueBase>>.Current => new KeyValuePair<TKey, TValueBase>(Current.Key, Current.Value);
        object IEnumerator.Current => ((IEnumerator) _enumeratorImplementation).Current;
    }
}