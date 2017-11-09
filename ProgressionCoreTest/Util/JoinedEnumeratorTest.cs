using System.Collections;
using System.Collections.Generic;
using Progression.Util;
using Xunit;

namespace Progression.Test.Engine.Core.Util
{
    public class JoinedEnumeratorTest
    {
        [Fact]
        public void TestEnumerator()
        {
            var e = new JoinedEnumerator<int>(new[] {0, 1, 2, 3}, new[] {4, 5, 6, 7}, new[] {8, 9, 10}, new[] {11, 12});
            var i = 0;
            foreach (var j in new JoinedEnumerable<int>(e)) {
                Assert.Equal(i++, j);
            }
        }
    }

    public class JoinedEnumerable<T> : IEnumerable<T>
    {
        public JoinedEnumerable(IEnumerator<T> enumerator)
        {
            Enumerator = enumerator;
        }


        public IEnumerator<T> Enumerator { get; }

        public IEnumerator<T> GetEnumerator() => Enumerator;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}