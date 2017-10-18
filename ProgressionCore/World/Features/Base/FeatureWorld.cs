using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Progression.Engine.Core.Util.BinPacking;
using Progression.Engine.Core.World.Features.Yield;

namespace Progression.Engine.Core.World.Features.Base
{
    public class FeatureWorld
    {
        private readonly List<IFeatureResolver> _features = new List<IFeatureResolver>();
        private readonly YieldManager _yieldManager;
        private readonly int[] _tileSize;
        
        public FeatureWorld(byte typesCount, YieldManager yieldManager)
        {
            _yieldManager = yieldManager;
            TypesCount = typesCount;
            _tileSize = new int[typesCount];
        }

        public bool Looked { get; private set; }
        public byte TypesCount { get; }
        public int GetTileSize(byte index) => _tileSize[index];
        


        public void Register(IFeatureResolver resolver) {
            _features.Add(resolver);
        }

        public void Lock()
        {
            //var bitsUsed = 0;
            var packets = new List<Packet>[TypesCount];
            for (var i = 0; i < packets.Length; i++) {
                packets[i] = new List<Packet>();
            }


            foreach (var resolver in _features) {
                //bitsUsed += resolver.Bits;
                foreach (var identifier in resolver.GenerateIdentifiers()) {
                    for (byte i = 0; i < identifier.WorldTypes.Size; i++) {
                        if (identifier.WorldTypes[i]) {
                            packets[i].Add(new Packet(identifier, identifier.Bits)); 
                        }
                    }
                }
            }

            //var vectorsOpt = (int) Math.Ceiling(bitsUsed / 32d);

            var binss = new List<Bin>[TypesCount];
            for (int i = 0; i < packets.Length; i++) {
                binss[i] = new List<Bin>();
                binss[i] = BinPackingSolvers.SolveStupid(packets[i], 32);
                _tileSize[i] = binss[i].Count;
            }
            
            //set data fields
            for (int i = 0; i < binss.Length; i++) {
                var bins = binss[i];

                for (ushort j = 0; j < bins.Count; j++) {
                    var bin = bins[j];
                    var lastSection = new BitVector32.Section();
                    var first = true;
                    foreach (var packet in bin) {
                        var identifier = (DataIdentifier) packet.Value;
                        if (first) {
                            lastSection = BitVector32.CreateSection((short) identifier.Bits);
                            first = false;
                        } else {
                            lastSection = BitVector32.CreateSection((short) identifier.Bits, lastSection);
                        }
                        DataLocation location = new DataLocation(j, lastSection);
                        if (identifier.Locations == null) {
                            identifier.Locations = new DataLocation[TypesCount];
                        }
                        identifier.Locations[i] = location;
                    }
                }

            }
            
            
            //give data to feature resolver
            foreach (var resolver in _features) {
                resolver.LockRegistration(this);
                
                AddITileYieldModifers(resolver);
            }
            
            _yieldManager.Lock();
            Looked = true;
        }

        private void AddITileYieldModifers(IFeatureResolver resolver)
        {
            if (resolver is ITileYieldModifer tileYieldModifier) {
                _yieldManager.AddTileYieldModifier(tileYieldModifier);
            } else foreach (var feature in resolver) {
                tileYieldModifier = feature as ITileYieldModifer;
                if (tileYieldModifier != null) {
                    _yieldManager.AddTileYieldModifier(tileYieldModifier);
                }
            }
        }
    }
}