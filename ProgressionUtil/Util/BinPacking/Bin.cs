using System.Collections;
using System.Collections.Generic;

namespace Progression.Util.BinPacking
{
    public class Bin : IEnumerable<Packet>
    {
        public int Used { get; private set; }
        private readonly List<Packet> _content = new List<Packet>();

        public void Store(Packet p)
        {
            Used += p.Size;
            _content.Add(p);
        }
        
        
        public IEnumerator<Packet> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}