using System;
using System.Collections.Generic;

namespace Progression.Util.Generics
{
    public class DoubleTypeDictionary<TKey, TValue, TValueBase> : Dictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValueBase> where TValue : TValueBase
    {
        private readonly IEnumerable<TValueBase> _values;
        
        public DoubleTypeDictionary()
        {
            if (typeof(TValue) == typeof(TValueBase)) throw new ArgumentException("TValueBase must be superclass of TValue", nameof(TValueBase));
            _values = new BaseTypeEnumerableWrapper<TValue, TValueBase>(Values);
        }

        IEnumerator<KeyValuePair<TKey, TValueBase>> IEnumerable<KeyValuePair<TKey, TValueBase>>.GetEnumerator()
        {
            return new BaseTypeKeyValuePairEnumeratorWrapper<TKey,TValue,TValueBase>(GetEnumerator());
        }

        bool IReadOnlyDictionary<TKey, TValueBase>.TryGetValue(TKey key, out TValueBase value)
        {
            var res = TryGetValue(key, out var temp);
            value = temp;
            return res;
        }

        TValueBase IReadOnlyDictionary<TKey, TValueBase>.this[TKey key] => this[key];

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValueBase>.Keys => Keys;

        IEnumerable<TValueBase> IReadOnlyDictionary<TKey, TValueBase>.Values => _values;
    }
}