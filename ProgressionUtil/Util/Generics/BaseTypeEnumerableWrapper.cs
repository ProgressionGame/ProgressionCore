using System.Collections;
using System.Collections.Generic;

namespace Progression.Util.Generics
{
    public class BaseTypeEnumerableWrapper<T, TBase> : IEnumerable<TBase> where T : TBase
    {
        private readonly IEnumerable<T> _enumerableImplementation;
        public BaseTypeEnumerableWrapper(IEnumerable<T> enumerableImplementation)
        {
            _enumerableImplementation = enumerableImplementation;
        }

        public IEnumerator<TBase> GetEnumerator()
        {
            return new BaseTypeEnumeratorWrapper<T, TBase>(_enumerableImplementation.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerableImplementation.GetEnumerator();
        }
    }
}