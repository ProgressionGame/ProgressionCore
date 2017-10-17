using System;
using System.Collections.Generic;

namespace Progression.Engine.Core.Util.BinPacking
{
    public static class BinPackingSolvers
    {
        public static List<Bin> SolveStupid(List<Packet> input, int maxSize)
        {
            input.Sort();
            var bins = new List<Bin>(input.Count); //may be too much, but no resizing

            while (input.Count > 0) {
                var cBin = new Bin();
                bins.Add(cBin);
                var indexNext = input.Count - 1;
                var spaceLeft = maxSize - cBin.Used;
                do {
                    var cPacket = input[indexNext];
                    input.RemoveAt(indexNext--); //removed one so decrements index
                    cBin.Store(cPacket);
                    spaceLeft -= cPacket.Size;
                    while (indexNext >= 0 && input[indexNext].Size > spaceLeft) indexNext--; //finds next fit
                } while (indexNext >= 0);
            }
            return bins;
        }
        public static List<Bin> SolveBestFitMaybe(List<Packet> input, int maxSize)
        {
            input.Sort();
            var bins = new List<Bin>(input.Count); //may be too much, but no resizing
            
            var cBin = new Bin();
            bins.Add(cBin);
            var index = input.Count - 1;

            while (input.Count > 0) {
                var cPacket = input[index];
                input.RemoveAt(index--);
                
                //searches for bin to store packet
                var binIndex = 0;
                var max = 0;
                for (var i = 0; i < bins.Count; i++) {
                    if (maxSize - bins[i].Used <= max) continue;
                    max = maxSize - bins[i].Used;
                    binIndex = i;
                }
                if (max < cPacket.Size) {
                    cBin = new Bin();
                    bins.Add(cBin);
                }
                else {
                    cBin = bins[binIndex];
                }
                
                
                cBin.Store(cPacket);

            }
            //input.RemoveAt(index); //do we even need to remove the last element (thinking emoji)
            return bins;
        }
    }
}