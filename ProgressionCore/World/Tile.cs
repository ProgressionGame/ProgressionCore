using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using Progression.Engine.Core.World.Features;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Yield;
using Progression.Engine.Core.World.Threading;

namespace Progression.Engine.Core.World
{
    public struct Tile
    {
        public readonly Coordinate Coordinate;
        public ushort X => Coordinate.X;
        public ushort Y => Coordinate.Y;
        
        public readonly TileWorld World;

        public Tile(Coordinate coord, TileWorld world)
        {
            Coordinate = coord;
            World = world;
        }

        public Tile(ushort x, ushort y, TileWorld world) : this(new Coordinate(x, y), world) {}

        public int this[DataIdentifier identifier] {
            get => World[X, Y, identifier];
            set => World[X, Y, identifier]=value;
        }

        public bool this[IFeature feature] {
            get => feature.HasFeature(this);
            set {
                if (value) feature.AddFeature(this);
                else feature.RemoveFeature(this);
            }
        }
        
        public bool HasFeature<T>(T feature) where T : class, IFeature<T>
        {
            return feature.HasFeature(this);
        }

        public void AddFeature<T>(T feature) where T : class, IFeature<T>
        {
            feature.AddFeature(this);
        }

        public void RemoveFeature<T>(T feature) where T : class, IFeature<T>
        {
            feature.RemoveFeature(this);
        }

        public double CalcYield(YieldType type)
        {
            return type.Manager.CalcYield(type, this);
        }

        public void InvokeTileUpdate<TFeature, TResolver>(TResolver resolver, TFeature feature, int newValue,
            DataIdentifier dataIdentifier)
            where TFeature : class, IFeature<TFeature>
            where TResolver : IFeatureResolver<TFeature>
        {
            World.ScheduleUpdate(new FeatureUpdate<TFeature, TResolver>(Coordinate, resolver, feature, newValue, dataIdentifier));
        }



        #region Operators
        public static bool operator ==(Tile a, Tile b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Tile a, Tile b)
        {
            return !(a == b);
        }

        public override bool Equals(object o)
        {
            if (o is Tile)
                return Equals((Tile) o);
            return false;
        }

        public override int GetHashCode()
        {
            unchecked {
                var hashCode = Coordinate.GetHashCode();
                hashCode = (hashCode * 397) ^ (World != null ? World.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(Tile t)
        {
            return Coordinate==t.Coordinate&&World==t.World;
        }
        #endregion  
    }
}