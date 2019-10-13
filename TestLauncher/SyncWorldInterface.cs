using System;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Threading;

namespace TestLauncher
{
    public class SyncWorldInterface : WorldInterface
    {
        public SyncWorldInterface(TileWorld world) : base(world) { }
        protected override bool ThreadWaiting => false;
        protected override void Notify()
        {
            throw new NotImplementedException();
        }

        public override void ScheduleUpdate(WorldUpdateBase update)
        {
            update.Execute(this);
        }

//        public override void GenericFeatureUpdate<TFeature, TResolver>(Coordinate coord, TResolver resolver, TFeature feature, int newValue,
//            int index, BitVector32.Section section)
//        {
//            base.GenericFeatureUpdate(coord, resolver, feature, newValue, index, section);
//            Console.WriteLine("w2 update: " + coord.X + ", " + coord.Y + " -> " + feature.Name);
//        }
    }
}