namespace Progression.IO
{
    public class PacketData
    {
        public int EstLength { get; private set; }
        public int Length => Data?.Length ?? 0;
        public byte[] Data{ get; private set; }
        public int Position{ get; private set; }

        public void Reset()
        {
            EstLength = 0;
            Data = null;
            Position = 0;
        }
        
        public void InitArray()
        {
            Data = new byte[EstLength];
            Position = 0;
        }
        
        public void Flip()
        {
            Position = 0;
        }

        public void AddEstimate(int i)
        {
            EstLength += i;
        }

        public void ShiftPosition(int i)
        {
            Position += i;
        }
    }
}