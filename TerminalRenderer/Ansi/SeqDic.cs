using System;
using System.Collections.Generic;

namespace Progression.TerminalRenderer.Ansi
{
    public class SeqDic : Dictionary<char, ISequenceTraverserPart>, ISequenceTraverserPart
    {
        
        public ISequenceTraverserPart GetNext(char current){
            TryGetValue(current, out var r);
            return r;
        }


        public AnsiInputEvent Traverse(AnsiInputConverter con, char c, bool b, bool qo, params object[] data) => con.NextPart(GetNext(c), b, qo, data);
    }
}