using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Progression.Engine.Core.Util
{
    public class JoinedEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T>[] _children;
        private int _index;
        public JoinedEnumerator(params IEnumerator<T>[] children)
        {
            _children = children;
        }

        public JoinedEnumerator(params IEnumerable<T>[] children) : this(childCollection: children)
        {}
        
        public JoinedEnumerator(IReadOnlyCollection<IEnumerable<T>> childCollection)
        {
            _children = new IEnumerator<T>[childCollection.Count];
            var i = 0;
            foreach (var child in childCollection) {
                _children[i++] = child.GetEnumerator();
            }
        }
        
        public void Dispose()
        {
            foreach (var child in _children) {
                child.Dispose();
            }
        }

        public bool MoveNext()
        {
            while (true) {
                if (_index >= _children.Length) return false;
                if (_children[_index].MoveNext()) return true;
                _index++;
            }
        }

        public void Reset()
        {
            _index = 0;
            foreach (var child in _children) {
                child.Reset();
            }
        }

        public T Current {
            get {
                if (_index > _children.Length)
                    throw new InvalidOperationException("undefined after last element was reached");
                return _children[_index].Current;
            }
        }

        object IEnumerator.Current => Current;
    }
}