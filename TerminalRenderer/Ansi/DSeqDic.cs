namespace Progression.TerminalRenderer.Ansi
{
    public class DSeqDic : ISequenceTraverserPart
    {
        public delegate ISequenceTraverserPart SeqDic(char c, params object[] data);

        private readonly SeqDic _seqDic;
        
        public DSeqDic(SeqDic seqDic)
        {
            _seqDic = seqDic;
        }
        

        public AnsiInputEvent Traverse(AnsiInputConverter con, char c, bool b, bool qo, params object[] data) => con.NextPart(GetNext(c, data), b, qo, data);
        public ISequenceTraverserPart GetNext(char current, params object[] data) => _seqDic(current, data);
    }
}